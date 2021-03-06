//
//  HeyzapUnitySDK.m
//
//  Copyright 2015 Smart Balloon, Inc. All Rights Reserved
//
//  Permission is hereby granted, free of charge, to any person
//  obtaining a copy of this software and associated documentation
//  files (the "Software"), to deal in the Software without
//  restriction, including without limitation the rights to use,
//  copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following
//  conditions:
//
//  The above copyright notice and this permission notice shall be
//  included in all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//  OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
//  HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//  WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
//

#import "HeyzapAds.h"
#import "HZInterstitialAd.h"
#import "HZVideoAd.h"
#import "HZIncentivizedAd.h"
#import "HZBannerAd.h"

extern void UnitySendMessage(const char *, const char *, const char *);

#define HZ_FRAMEWORK_NAME @"unity3d"

#define HZ_VIDEO_KLASS @"HZVideoAd"
#define HZ_INTERSTITIAL_KLASS @"HZInterstitialAd"
#define HZ_INCENTIVIZED_KLASS @"HZIncentivizedAd"
#define HZ_BANNER_KLASS @"HZBannerAd"

@interface HeyzapUnityAdDelegate : NSObject<HZAdsDelegate,HZIncentivizedAdDelegate,HZBannerAdDelegate>

@property (nonatomic, strong) NSString *klassName;

- (id) initWithKlassName: (NSString *) klassName;
- (void) sendMessageForKlass: (NSString *) klass withMessage: (NSString *) message andTag: (NSString *) tag;

@end

@implementation HeyzapUnityAdDelegate

- (id) initWithKlassName: (NSString *) klassName {
    self = [super init];
    if (self) {
        _klassName = klassName;
    }
    
    return self;
}

- (void) didReceiveAdWithTag:(NSString *)tag { [self sendMessageForKlass: self.klassName withMessage: @"available" andTag: tag]; }

- (void) didFailToReceiveAdWithTag:(NSString *)tag { [self sendMessageForKlass: self.klassName withMessage: @"fetch_failed" andTag: tag]; }

- (void) didShowAdWithTag:(NSString *)tag { [self sendMessageForKlass: self.klassName withMessage: @"show" andTag: tag]; }

- (void) didHideAdWithTag:(NSString *)tag { [self sendMessageForKlass: self.klassName withMessage:  @"hide" andTag: tag]; }

- (void) didFailToShowAdWithTag:(NSString *)tag andError:(NSError *)error { [self sendMessageForKlass: self.klassName withMessage:  @"failed" andTag: tag]; }

- (void) didClickAdWithTag:(NSString *)tag { [self sendMessageForKlass: self.klassName withMessage:  @"click" andTag: tag]; }

- (void) didCompleteAdWithTag: (NSString *) tag { [self sendMessageForKlass: self.klassName withMessage:  @"incentivized_result_complete" andTag: tag]; }

- (void) didFailToCompleteAdWithTag: (NSString *) tag { [self sendMessageForKlass: self.klassName withMessage:  @"incentivized_result_incomplete" andTag: tag]; }

- (void) willStartAudio { [self sendMessageForKlass: self.klassName  withMessage: @"audio_starting" andTag:  @""]; }

- (void) didFinishAudio { [self sendMessageForKlass: self.klassName withMessage:  @"audio_finished" andTag:  @""]; }

- (void)bannerDidReceiveAd:(HZBannerAd *)banner {
    [self sendMessageForKlass:self.klassName withMessage:@"loaded" andTag:banner.options.tag];
}

- (void)bannerDidFailToReceiveAd:(HZBannerAd *)banner error:(NSError *)error {
    if (banner != nil) {
        [self sendMessageForKlass:self.klassName withMessage:@"error" andTag:banner.options.tag];
    } else {
        [self sendMessageForKlass:self.klassName withMessage: @"error" andTag: @""];
    }
}

- (void)bannerWasClicked:(HZBannerAd *)banner {
    [self sendMessageForKlass:self.klassName withMessage:@"click" andTag:banner.options.tag];
}

- (void) sendMessageForKlass: (NSString *) klass withMessage: (NSString *) message andTag: (NSString *) tag {
    NSString *unityMessage = [NSString stringWithFormat: @"%@,%@", message, tag];
    UnitySendMessage([klass UTF8String], "setDisplayState", [unityMessage UTF8String]);
}

@end

static HeyzapUnityAdDelegate *HZInterstitialDelegate = nil;
static HeyzapUnityAdDelegate *HZIncentivizedDelegate = nil;
static HeyzapUnityAdDelegate *HZVideoDelegate = nil;
static HeyzapUnityAdDelegate *HZBannerDelegate = nil;

static HZBannerAd *HZCurrentBannerAd = nil;

extern "C" {
    void hz_ads_start_app(const char *publisher_id, HZAdOptions flags) {
        NSString *publisherID = [NSString stringWithUTF8String: publisher_id];
        
        [HeyzapAds startWithPublisherID: publisherID andOptions: flags andFramework: HZ_FRAMEWORK_NAME];
        
        HZIncentivizedDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName: HZ_INCENTIVIZED_KLASS];
        [HZIncentivizedAd setDelegate: HZIncentivizedDelegate];
        
        HZInterstitialDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName: HZ_INTERSTITIAL_KLASS];
        [HZInterstitialAd setDelegate: HZInterstitialDelegate];
        
        HZVideoDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName: HZ_VIDEO_KLASS];
        [HZVideoAd setDelegate: HZVideoDelegate];
        
        HZBannerDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName: HZ_BANNER_KLASS];

        [HeyzapAds networkCallbackWithBlock:^(NSString *network, NSString *callback) {
            NSString *unityMessage = [NSString stringWithFormat: @"%@,%@", network, callback];
            NSString *klassName = @"HeyzapAds";
            UnitySendMessage([klassName UTF8String], "setNetworkCallbackMessage", [unityMessage UTF8String]);
        }];
    }
    
    void hz_ads_show_interstitial(const char *tag) {
        [HZInterstitialAd showForTag: [NSString stringWithUTF8String: tag]];
    }
    
    void hz_ads_hide_interstitial(void) {
        //[HZInterstitialAd hide];
    }
    
    void hz_ads_fetch_interstitial(const char *tag) {
        [HZInterstitialAd fetchForTag: [NSString stringWithUTF8String: tag]];
    }
    
    bool hz_ads_interstitial_is_available(const char *tag) {
        return [HZInterstitialAd isAvailableForTag: [NSString stringWithUTF8String: tag]];
    }
    
    void hz_ads_show_video(const char *tag) {
        [HZVideoAd showForTag: [NSString stringWithUTF8String: tag]];
    }
    
    void hz_ads_hide_video(void) {
        //[HZVideoAd hide];
    }
    
    void hz_ads_fetch_video(const char *tag) {
        [HZVideoAd fetchForTag: [NSString stringWithUTF8String: tag]];
    }
    
    bool hz_ads_video_is_available(const char *tag) {
        return [HZVideoAd isAvailable];
    }
    
    void hz_ads_show_incentivized(const char *tag) {
        [HZIncentivizedAd showForTag: [NSString stringWithUTF8String: tag]];
    }
    
    void hz_ads_hide_incentivized() {
        //[HZIncentivizedAd hide];
    }
    
    void hz_ads_fetch_incentivized(const char *tag) {
        [HZIncentivizedAd fetchForTag: [NSString stringWithUTF8String: tag]];
    }
    
    bool hz_ads_incentivized_is_available(const char *tag) {
        return [HZIncentivizedAd isAvailableForTag: [NSString stringWithUTF8String: tag]];
    }
    
    void hz_ads_incentivized_set_user_identifier(const char *identifier) {
        NSString *userID = [NSString stringWithUTF8String:identifier];
        if ([userID isEqualToString:@""]) {
            userID = nil;
        }
        return [HZIncentivizedAd setUserIdentifier: userID];
    }
        
    void hz_ads_show_banner(const char *position, const char *tag) {
        if (HZCurrentBannerAd == nil) {
            HZBannerPosition pos = HZBannerPositionBottom;
            NSString *positionStr = [NSString stringWithUTF8String: position];
            if ([positionStr isEqualToString: @"top"]) {
                pos = HZBannerPositionTop;
            }
            
            HZBannerAdOptions *options = [[HZBannerAdOptions alloc] init];
            options.tag = [NSString stringWithUTF8String:tag];
            
            [HZBannerAd placeBannerInView:nil position:pos options:options success:^(HZBannerAd *banner) {
                HZCurrentBannerAd = banner;
                [HZCurrentBannerAd setDelegate: HZBannerDelegate];
            } failure:^(NSError *error) {
                NSLog(@"Error fetching banner; error = %@",error);
                [HZBannerDelegate bannerDidFailToReceiveAd: nil error: error];
            }];
        } else {
            // Unhide the banner
            [HZCurrentBannerAd setHidden: NO];
        }
    }
    
    void hz_ads_hide_banner(void) {
        if (HZCurrentBannerAd != nil) {
            [HZCurrentBannerAd setHidden: YES];
        }
    }

    void hz_ads_destroy_banner(void) {
        if (HZCurrentBannerAd  != nil) {
            [HZCurrentBannerAd removeFromSuperview];
            HZCurrentBannerAd = nil;
        }
    }
    
    void hz_ads_show_mediation_debug_view_controller(void) {
        [HeyzapAds presentMediationDebugViewController];
    }

    bool hz_ads_is_network_initialized(const char *network) {
        return [HeyzapAds isNetworkInitialized: [NSString stringWithUTF8String: network]];
    }
}

#!/usr/bin/python
import os
#from Cocoa import NSDictionary
import sys
import re
from distutils import dir_util

import PbxprojHZ

try:
  for proj in os.listdir(sys.argv[1]):
    if not re.search('\.xcodeproj', proj):
      continue

    proj = os.path.join(sys.argv[1], proj)
    pb = PbxprojHZ.PbxprojHZ(proj)

    # Enable modules for the benefit of AdMob.
    # (This allows automatic linking for the frameworks they use)
    pb.add_build_setting("Debug",  "CLANG_ENABLE_MODULES","YES")
    pb.add_build_setting("Release","CLANG_ENABLE_MODULES","YES")

    # Add -ObjC for the benefit of AppLovin/FAN
    pb.add_build_setting("Debug",   "OTHER_LDFLAGS", "-ObjC")
    pb.add_build_setting("Release", "OTHER_LDFLAGS", "-ObjC")

    # Add the Frameworks
    pb.add_framework("AdSupport.framework", True) #weak-linked
    pb.add_framework("StoreKit.framework", True) #weak-linked
    pb.add_framework("CoreGraphics.framework")
    pb.add_framework("QuartzCore.framework")
    pb.add_framework("CoreTelephony.framework")
    pb.add_framework("iAd.framework")
    pb.add_framework("MobileCoreServices.framework")
    pb.add_framework("Security.framework")
    
except:
  e = sys.exc_info()[0]

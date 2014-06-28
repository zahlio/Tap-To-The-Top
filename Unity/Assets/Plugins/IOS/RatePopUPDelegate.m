//
//  RatePopUPDelegate.m
//
//  Created by Osipov Stanislav on 1/12/13.
//
//

#import "RatePopUPDelegate.h"
#import "ISNDataConvertor.h"
#import "IOSNativeUtility.h"
#import "IOSNativePopUpsManager.h"

@implementation RatePopUPDelegate


NSString *reviewURLTempalte = @"itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=APP_ID";

NSString* reviewURLTemplateIOS7  = @"itms-apps://itunes.apple.com/app/idAPP_ID";


- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex {
    
    
    
    [IOSNativePopUpsManager unregisterAllertView];
    UnitySendMessage("IOSRateUsPopUp", "onPopUpCallBack",  [ISNDataConvertor NSIntToChar:buttonIndex]);
}


@end

using UnityEngine;
using System.Collections;

public class ads : MonoBehaviour {
	
	private iAdBanner banner1;
	
	private bool IsInterstisialsAdReady = false;
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	void Start() {
		
		iAdBannerController.instance.addEventListener(iAdEvent.INTERSTITIAL_AD_DID_LOAD, OnInterstisialsLoaded);
		iAdBannerController.instance.addEventListener(iAdEvent.INTERSTITIAL_AD_ACTION_DID_FINISH, OnInterstisialsFinish);

		banner1 = iAdBannerController.instance.CreateAdBanner(TextAnchor.LowerCenter);
		banner1.ShowOnLoad = true;
	}

	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	private void OnInterstisialsLoaded() {
		IsInterstisialsAdReady = true;
	}
	
	private void OnInterstisialsFinish() {
		IsInterstisialsAdReady = false;
	}
	
	
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------
	
}

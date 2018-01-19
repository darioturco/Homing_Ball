using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;	
public class UnityADs : MonoBehaviour {
	public string id;
	public bool edit; 
	private bool _startAds = false;
	void Awake () {
		#if UNITY_IOS || UNITY_ANDROID
		Advertisement.Initialize(id, edit);                          
		#endif
	}
	void Update () {
		#if UNITY_IOS || UNITY_ANDROID
		if (Advertisement.isShowing==false && _startAds==true){
			_startAds = false;
		}
		#endif
	}
	public void ShowAds(){ // metodo externo para llamar a las ads
		#if UNITY_IOS || UNITY_ANDROID
		StartCoroutine(ShowAdWhenReady());
		#endif
	}
	IEnumerator ShowAdWhenReady(){
		while (!Advertisement.IsReady())
			yield return null;
		Advertisement.Show();
		_startAds = true;		
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Boton : MonoBehaviour, IPointerDownHandler,  IPointerUpHandler{
	public bool pulsa, down, up;
	private bool pulsa_aux;
	public void OnPointerDown (PointerEventData eventData) {
		pulsa = true;
	}
	public void OnPointerUp (PointerEventData eventData) {
		pulsa = false;
	}
	void OnEnable () {
		pulsa = false;
		pulsa_aux = false;
		down = false;
		up = false;
	}
	void OnDisable () {
		pulsa = false;
		pulsa_aux = false;
		down = false;
		up = false;
	}
	void Update () {
		if(pulsa_aux == false && pulsa == true){
			down = true;
		}else{
			if(down == true){
				down = false;
			}
		}
		if(pulsa_aux == true && pulsa == false){
			up = true;
		}else{
			if(up == true){
				up = false;
			}
		}
		pulsa_aux = pulsa;
	}
}
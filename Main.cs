using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Main : MonoBehaviour {//7/1/2018
	public GameObject pelota, flecha, aux, objetivo, menu_obj, gana_obj;
	public GameObject[] obj, dest;
	public Boton vel_x, menu, reset, cancel, nuevo_map, compartir, nuevo_map_v, atras_v, press_bot, rojo, verde, azul;
	public Text gana_text, start_text, menu_text;
	public Image vel_img, reset_img, press_img, gana_img, menu_img, map1_img, can_img, dif1_img, map2_img, vol_img, dif2_img, comp_img;
	public Color[] claro, oscuro, solido, slider, victoria;
	public Slider dif_sld_menu, dif_sld_gana;
	public ParticleSystem part;
	public Animator anim;
	public AudioSource menu_aud, pelota_aud, objet_aud;
	public AudioClip bot_son;
	public LayerMask mask;
	private CircleCollider2D cir;
	private Rigidbody2D rigid;
	private bool pulsa, disparo, move, pausa, gano, press, isProcessing, isFocus;
	private int i, cont, difi;
	private float ang, max, mid_x, pos_y, time;
	private Vector2 ini, fin, pun;

	void Awake () {
		cir = objetivo.GetComponent<CircleCollider2D>();
		rigid = pelota.GetComponent<Rigidbody2D>();
		ini = pelota.transform.position;
		flecha.transform.position = ini;
		max = GameObject.Find("Arriba").transform.position.y-3.4f;
		mid_x = 5.75f;
		difi = 6;

		/*borrar*/
		string aus_s = Application.systemLanguage.ToString();
		start_text.text = aus_s;
		//Debug.Log(aus_s);
	}
	IEnumerator crea_pro(float inicio, float dist, int max_obj, bool col, bool res){//inicion = poscision en la que los bloques empiezan a generarse
		disparo = true;//dist = distacia entre los bloques en el eje y
		pun = ini;//max_obj = cantidad maxima de objetos por linea
		pun.x = Random.Range(-mid_x,mid_x);//col = si pueden los objetos estar en el mismo lugar
		pun.y += inicio;//res = cuando se chocan sube de linea
		cir.enabled = true;
		rigid.velocity = Vector2.zero;
		objetivo.transform.position = new Vector2(Random.Range(-5.00f,5.00f),objetivo.transform.position.y);
		cont = 0;
		while(pun.y < max){
			for(i=0;i<max_obj;i++){
				aux = Instantiate(obj[Random.Range(0,5)]);
				aux.transform.SetPositionAndRotation(pun,Quaternion.Euler(0,0,Random.Range(0,180)));
				pun.x = Random.Range(-mid_x,mid_x);
				if(col == false){
					yield return new WaitForSeconds(0.02f);
					if(aux != null){
						if(aux.GetComponent<Collider2D>().IsTouchingLayers(mask)){
							DestroyImmediate(aux);
							if(res == true){
								i = max_obj++;
							}
						}
					}
				}
				if(aux != null){
					dest[cont] = aux;
					cont++;
				}
			}
			pun.y += dist;
		}
		cir.enabled = false;
		yield return new WaitForSeconds(0.025f);
		disparo = false;
	}
	IEnumerator time_cor(){
		time = 0;
		while(true){
			yield return new WaitForSeconds(0.01f);
			if(vel_x.pulsa == true){
				time += 0.02f;
			}else{
				time += 0.01f;
			}
		}
	}
	IEnumerator reset_cor(){
		StopCoroutine("time_cor");
		rigid.velocity = Vector2.zero;
		move = true;
		for(i=0;i<20;i++){
			pelota.transform.position = Vector2.Lerp(pelota.transform.position,ini,i*0.05f);//mueve la peloa hasta el inicio
			yield return new WaitForSeconds(0.01f/(i+1));
		}
		disparo = false;
		move = false;
	}
	IEnumerator press_cor(){
		yield return new WaitForSeconds(menu_aud.clip.length);
		menu_aud.clip = bot_son;
		ajusta_nivel();
		press = true;
	}
	public void ajusta_nivel(){
		switch(difi){
		default:
			StartCoroutine(crea_pro(3.50f,5.00f,1,false,true));
			break;
		case 1:
			StartCoroutine(crea_pro(3.25f,4.00f,2,false,true));
			break;
		case 2:
			StartCoroutine(crea_pro(3.25f,3.50f,2,false,true));
			break;
		case 3:
			StartCoroutine(crea_pro(3.00f,3.50f,3,false,true));
			break;
		case 4:
			StartCoroutine(crea_pro(2.75f,3.00f,3,false,true));
			break;
		case 5:
			StartCoroutine(crea_pro(2.75f,2.50f,3,false,true));
			break;
		case 6:
			StartCoroutine(crea_pro(2.75f,2.50f,4,false,true));
			break;
		case 7:
			StartCoroutine(crea_pro(2.70f,2.40f,4,false,false));
			break;
		case 8:
			StartCoroutine(crea_pro(2.60f,2.25f,5,false,false));
			break;
		case 9:
			StartCoroutine(crea_pro(2.50f,2.00f,6,false,false));
			break;
		case 10:
			StartCoroutine(crea_pro(2.40f,1.5f,7,false,false));
			break;
		}
	}
	void cambia_color(int num){
		start_text.color = new Color(solido[num].r,solido[num].g,solido[num].b,start_text.color.a);
		menu_text.color = new Color(claro[num].r,claro[num].g,claro[num].b,menu_text.color.a);
		vel_img.color = new Color(claro[num].r,claro[num].g,claro[num].b,vel_img.color.a);
		reset_img.color = new Color(claro[num].r,claro[num].g,claro[num].b,reset_img.color.a);
		press_img.color = new Color(oscuro[num].r,oscuro[num].g,oscuro[num].b,press_img.color.a);
		gana_img.color = new Color(oscuro[num].r,oscuro[num].g,oscuro[num].b,gana_img.color.a);
		menu_img.color = new Color(oscuro[num].r,oscuro[num].g,oscuro[num].b,menu_img.color.a);
		map1_img.color = new Color(solido[num].r,solido[num].g,solido[num].b,map1_img.color.a);
		can_img.color = new Color(solido[num].r,solido[num].g,solido[num].b,can_img.color.a);
		dif1_img.color = new Color(solido[num].r,solido[num].g,solido[num].b,dif1_img.color.a);
		map2_img.color = new Color(solido[num].r,solido[num].g,solido[num].b,map2_img.color.a);
		vol_img.color = new Color(solido[num].r,solido[num].g,solido[num].b,vol_img.color.a);
		dif2_img.color = new Color(solido[num].r,solido[num].g,solido[num].b,dif2_img.color.a);
		comp_img.color = new Color(solido[num].r,solido[num].g,solido[num].b,comp_img.color.a);
		gana_text.color = new Color(victoria[num].r,victoria[num].g,victoria[num].b,gana_text.color.a);
	}
	public void ShareBtnPress(){
		if(!isProcessing){
			StartCoroutine(ShareScreenshot());
		}
	}
	IEnumerator ShareScreenshot(){
		isProcessing = true;
		yield return new WaitForEndOfFrame();
		ScreenCapture.CaptureScreenshot("screenshot.png", 2);
		string destination = Path.Combine(Application.persistentDataPath, "screenshot.png");
		yield return new WaitForSecondsRealtime(0.3f);
		if (!Application.isEditor){
			AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
			AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Mira este increible juego: Homing Ball");
			intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your new score");
			currentActivity.Call("startActivity", chooser);
			yield return new WaitForSecondsRealtime(1);
		}
		yield return new WaitUntil(() => isFocus);
		isProcessing = false;
	}
	private void OnApplicationFocus (bool focus) {
		isFocus = focus;
	}
	void Update () {
		if(press == true){
			if(gano == true){
				if(atras_v.up == true){
					gana_obj.SetActive(false);
					objetivo.SetActive(true);
					pelota.transform.position = new Vector2(pelota.transform.position.x,pelota.transform.position.y-1);
					StartCoroutine("reset_cor");
					gano = false;
					menu_aud.Play();
				}
				if(nuevo_map_v.up == true){
					for(i=0;i<dest.Length;i++){
						DestroyImmediate(dest[i]);//limpia los objetos
					}
					ajusta_nivel();
					pelota.transform.position = ini;
					gano = false;
					menu_aud.Play();
					gana_obj.SetActive(false);//desactiva el menu de victoria
					objetivo.SetActive(true);//reinicia el objetivo
				}
				if(compartir.up == true){//Comparte con las redes sociales
					ShareBtnPress();
				}
				difi = Mathf.RoundToInt(dif_sld_gana.value);
			}else{
				if(Physics2D.OverlapCircle(objetivo.transform.position,0.38f,LayerMask.GetMask("Pelota"))){//si toca el objetivo
					rigid.velocity = Vector2.zero;
					part.transform.position = objetivo.transform.position;
					part.Play();
					StopCoroutine("time_cor");
					gana_text.text = "Felicidades! \nGanaste en " + System.Math.Round(time,3) + " segundos";
					objetivo.SetActive(false);
					menu_obj.SetActive(false);
					gana_obj.SetActive(true);
					objet_aud.Play();
					dif_sld_gana.value = difi;
					pausa = false;
					gano = true;
				}
				if(pausa == true){
					if(nuevo_map.up == true){//crea un nuevo mapa
						for(i=0;i<dest.Length;i++){
							DestroyImmediate(dest[i]);//limpia los objetos
						}
						ajusta_nivel();
						pelota.transform.position = ini;
						pausa = false;
						menu_aud.Play();
						menu_obj.SetActive(false);
					}
					if(cancel.up == true){//sale del menu
						pausa = false;
						menu_aud.Play();
						menu_obj.SetActive(false);
					}
					if(rojo.up == true){
						cambia_color(0);//rojo
					}
					if(verde.up == true){
						cambia_color(1);//verde
					}
					if(azul.up == true){
						cambia_color(2);//azul
					}
					difi = Mathf.RoundToInt(dif_sld_menu.value);
				}else{
					if(menu.up == true){//entra al menu
						pausa = true;
						menu_aud.Play();
						menu_obj.SetActive(true);
						dif_sld_menu.value = difi;
					}
					if(disparo == false){
						if(Input.GetMouseButton(0)){
							if(flecha.activeSelf == false){
								flecha.SetActive(true);//flecha activada
							}
							fin = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);//obtiene la pocion en donde se toco la pantalla
							pos_y = fin.y;
							fin -= ini;
							ang = Mathf.Atan(fin.y/fin.x);
							ang = (180*ang)/Mathf.PI;
							if(ang < 0){
								ang += 180;
							}//calcula donde apunta la flecha y su escala
							flecha.transform.localScale = new Vector2(Mathf.Clamp(Mathf.Sqrt(Mathf.Pow(fin.x,2)+Mathf.Pow(fin.y,2))/10,0.00f,0.6f),1);
							flecha.transform.localScale = new Vector2(flecha.transform.localScale.x,flecha.transform.localScale.x);
							flecha.transform.rotation = Quaternion.Euler(0,0,ang);
						}
						if(Input.GetMouseButtonUp(0)){
							if(pos_y <= max){
								ang = ang/Mathf.Rad2Deg;
								fin = new Vector2(18*Mathf.Cos(ang),18*Mathf.Sin(ang));
								disparo = true;
								rigid.velocity = fin;//bola enviada
								pelota_aud.Play();
								StartCoroutine("time_cor");
							}
							flecha.SetActive(false);//flecha desactivada
						}
					}else{
						if(rigid.velocity.x != 0 && rigid.velocity.y != 0){
							if(vel_x.down == true){//duplica la velocidad cuando se apreta el boton de acelerar
								rigid.velocity *= 2;
							}
							if(vel_x.up == true){//reduce la velocidad cuando se levanta el boton de acelerar
								rigid.velocity /= 2;
							}
							if(reset.up == true){//resetea la pelota a su poscision original
								StartCoroutine("reset_cor");
							}
						}
					}
				}
				if(pelota.transform.position.y < ini.y-0.5f && move == false){//perdio y se resetea
					StartCoroutine("reset_cor");
				}
			}
		}else{
			if(press_bot.up == true){
				anim.SetBool("ok",true);
				menu_aud.Play();
				StartCoroutine("press_cor");
			}
		}
	}
}
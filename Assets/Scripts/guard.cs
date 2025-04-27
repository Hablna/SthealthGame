using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guard : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;
    public Transform pathHolder;
    public float speed;
    public float viewDistance;
    public Light spotLight; 
    public Transform player;
    private Color normalcolor;

    void Start()
    {
        //positionne le guard sur le premier waypoint
        transform.position = pathHolder.GetChild(0).position;


        if(spotLight == null){
            spotLight = GetComponent<Light>();
        }
        //demarre la coroutine pour patrouiller entre les waypoints
        StartCoroutine(MyLoopWait());
        normalcolor = spotLight.color;
    }

    void Update(){
        //regarde si le joueur est detecté
        if (IsPlayerDetected()){
            spotLight.color = Color.red;

            if (OnGuardHasSpottedPlayer != null)
            {
                OnGuardHasSpottedPlayer();
            }
        }else{
            spotLight.color = normalcolor;
        }
    }

    void OnDrawGizmos(){
        //position de départ pour definir les lignes entre les waypoints
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        //dessine les spheres et les lignes entre les waypoints
        foreach (Transform waypoint in pathHolder ){
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(waypoint.position, .2f);
            Debug.DrawLine (previousPosition, previousPosition = waypoint.position, Color.blue);

        }
        // Dessine une ligne entre le dernier waypoint et le premier pour boucler
        Debug.DrawLine (previousPosition, startPosition, Color.blue);

        //Dessine un rayon rouge pour representer la distance de vision du guard
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    // Coroutine pour déplacer le garde vers une destination donnée
    IEnumerator MyLoop (UnityEngine.Vector3 destination){
        // Debug.Log(destination);
        Quaternion targetRotation = Quaternion.LookRotation(destination - transform.position);
        
        //Tourne le garde vers la destination
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 30 * Time.deltaTime);
            yield return null;
        }

        //Déplace le garde vers la destination
        while (Vector3.Distance(transform.position, destination) > .1f){
            transform.position = Vector3.MoveTowards(transform.position, destination, speed*Time.deltaTime);
            Debug.Log(transform.position);
            yield return null;
        }
    }


    IEnumerator MyLoopWait(){
       while (true){
            //parcourt chaque waypoint et déplace le garde vers celui-ci
            foreach(Transform waypoint in pathHolder ){
                yield return StartCoroutine(MyLoop(waypoint.position));
            }
        }
    
    }

    bool IsPlayerDetected(){
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angleGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
        
        //Imbrication des conditions pour detecter le joueur
        if(angleGuardAndPlayer < spotLight.spotAngle / 2){
            if (Vector3.Distance(transform.position, player.position) < viewDistance){
                RaycastHit hit;
                if (Physics.Raycast(transform.position, dirToPlayer, out hit,viewDistance)){
                    if (hit.transform == player){
                        return true;
                    }
                }
            }
        }

        return false;
    }

}

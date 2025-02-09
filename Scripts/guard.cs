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

    // Start is called before the first frame update
    void Start()
    {
        transform.position = pathHolder.GetChild(0).position;
        if(spotLight == null){
            spotLight = GetComponent<Light>();
        }

        StartCoroutine(MyLoopWait());
        normalcolor = spotLight.color;
    }
    void Update(){
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
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder ){
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(waypoint.position, .2f);
            Debug.DrawLine (previousPosition, previousPosition = waypoint.position, Color.blue);

        }
        Debug.DrawLine (previousPosition, startPosition, Color.blue);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    IEnumerator MyLoop (UnityEngine.Vector3 destination){
        Quaternion targetRotation = Quaternion.LookRotation(destination - transform.position);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 30 * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, destination) > .1f){
            transform.position = Vector3.MoveTowards(transform.position, destination, speed*Time.deltaTime);
            yield return null;
        }
        
    }
    IEnumerator MyLoopWait(){
       while (true){
            foreach(Transform waypoint in pathHolder ){
                yield return StartCoroutine(MyLoop(waypoint.position));
            }
        }
    
    }

    bool IsPlayerDetected(){
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angleGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;



public class FocusScript : MonoBehaviour
{
    // Start is called before the first frame update

    public const float scaleFactor = 500f;

    Texture2D cloudAtlas;

    Image cloudAtlasImage;

    GameObject myImage;

    CloudEmitter emitter;


    public Vector2 screenCenter;

 

    private List<Subject> subjects;

    static public void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    void Start()
    {
        subjects = new List<Subject>();
        Application.targetFrameRate = 60;
        byte[] cloudAtlasFileData = File.ReadAllBytes(Application.dataPath + "/Focus_Assets/clouds.png");
        cloudAtlas = new Texture2D(4096,512);
        cloudAtlas.LoadImage(cloudAtlasFileData);

        GameObject self = GameObject.Find("Canvas");

        screenCenter = new Vector2(self.GetComponent<RectTransform>().rect.width/2,self.GetComponent<RectTransform>().rect.height/2);


        /*
        myImage = new GameObject();
        myImage.transform.parent = self.transform;

        cloudAtlasImage = myImage.AddComponent<Image>();
        Sprite cloud1 = Sprite.Create(cloudAtlas,new Rect(0,0,512,512),new Vector2(0,0));

        cloudAtlasImage.sprite = cloud1;



        RectTransform transform = cloudAtlasImage.GetComponent<RectTransform>();
        transform.position = new Vector2(100,600);*/


        emitter = new CloudEmitter(self,cloudAtlas);
        emitter.localMaximumPoint = new Vector2(450,70);

        /*
        Subject temp = new Subject(self);
        temp.transform.transform.position = new Vector2(270,-400) + screenCenter;
        temp.SetFraction(0.3f);
        subjects.Add(temp);

        temp = new Subject(self);
        temp.transform.transform.position = new Vector2(0,400) + screenCenter;
        temp.SetFraction(0.3f);
        subjects.Add(temp);

        

        temp = new Subject(self);
        temp.transform.transform.position = new Vector2(-300,-300) + screenCenter;
        temp.SetFraction(0.35f);
        subjects.Add(temp);

        temp = new Subject(self);
        temp.transform.transform.position = new Vector2(600,-50) + screenCenter;
        temp.SetFraction(0.05f);
        
        subjects.Add(temp);


        ChangeFocus(3,0.7f);*/
        

    }


    // Update is called once per frame
    void Update()
    {
        
        for(int i=0; i<subjects.Count; i++){
            subjects[i].SetAdjusted(CenterOfMass());
            
            subjects[i].Iterate();
        }
        
        //emitter2.Iterate();
        emitter.Iterate();
    }

    public Vector2 CenterOfMass()
    {
        Vector2 output = new Vector2(0,0);


        for(int i=0; i<subjects.Count; i++){
            output.x += subjects[i].fraction * subjects[i].transform.position.x;
            output.y += subjects[i].fraction * subjects[i].transform.position.y;
        }

        

        return output;
    }

    public void ChangeFocus(int index, float delta)
    {
        subjects[index].fractionGoal = subjects[index].fraction + delta;
        if(subjects.Count > 1){
            float sub = -1 * delta / (subjects.Count - 1);
            for(int i=0; i<subjects.Count; i++){
                if(i!=index){
                    subjects[i].fractionGoal = subjects[i].fraction + sub;
                }
            }
        }
        
    }
}

public class Subject
{
    private Vector2 screenCenter;
    public Vector2 adjustedCenter;

    public float fractionGoal;
    public void SetAdjusted(Vector2 v)
    {
        adjustedCenter = v;
    }
    public Subject(GameObject Canvas)
    {
        graphics = new SubjectGraphics();
        Object = GameObject.Instantiate(GameObject.Find("Subject"));
        
        

        collider = Object.GetComponent<CircleCollider2D>();
        
        rigidbody = Object.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;

        

        

        GameObject canvas = GameObject.Find("Canvas");
        screenCenter = new Vector2(canvas.GetComponent<RectTransform>().rect.width/2,canvas.GetComponent<RectTransform>().rect.height/2);

        Object.transform.parent = Canvas.transform;


        im = Object.AddComponent<Image>();

        transform = Object.GetComponent<RectTransform>();
        float r = 300;
        transform.sizeDelta = new Vector2(r*2,r*2);
        collider.radius = r;

        SetFraction(0.3f);
        
        Texture2D tex = new Texture2D((int)r*2,(int)r*2);
        for(int x=0; x<(int)r * 2; x++){
            for(int y=0; y<(int)r * 2; y++){
                if((new Vector2(x,y) - new Vector2((int)r,(int)r)).magnitude <= r){
                    tex.SetPixel(x,y,Color.blue);
                }
                else{
                    tex.SetPixel(x,y,new Color(0,0,0,0));
                }
                
            }
        }
        tex.Apply();
        Sprite s = Sprite.Create(tex,new Rect(0,0,(int)r*2,(int)r*2),new Vector2(0,0));
        im.sprite = s;
        
        

    }

    public GameObject Object;

    public CircleCollider2D collider;

    public Rigidbody2D rigidbody;

    public Image im;

    public RectTransform transform;

    public float fraction = 0.1f;
    public float radius = 0.1784f;
    public void SetFraction(float f){
        fraction = f;
        radius = Mathf.Sqrt(f / Mathf.PI);
        transform.localScale = new Vector2(radius * 1.77245f,radius * 1.77245f);
        

    }


    public string subject_name = "void";

    private SubjectGraphics graphics;


    public float maximum_velocity = 100;
    public void IterateGravity()
    {
        Vector2 adjusted = screenCenter * 2 - adjustedCenter ;
        Vector2 acceleration = (adjusted - new Vector2(Object.GetComponent<RectTransform>().transform.position.x,Object.GetComponent<RectTransform>().transform.position.y));
        if(rigidbody.velocity.magnitude < maximum_velocity){
            rigidbody.AddForce(acceleration);
        }
        rigidbody.velocity*=0.9f;
        
    }

    public void Iterate()
    {
        if(fraction < fractionGoal){
            if(Mathf.Abs(fraction-fractionGoal)<0.0006){
                SetFraction(fractionGoal);
            }else{
                SetFraction(fraction+0.0003f);
            }
            
        }
        if(fraction>fractionGoal){
            if(Mathf.Abs(fraction-fractionGoal)<0.0006){
                SetFraction(fractionGoal);
            }else{
                SetFraction(fraction-0.0003f);
            }
        }
        IterateGravity();
        graphics.Iterate();
    }
}

public class SubjectGraphics
{
    public SubjectGraphics()
    {
        emitters = new List<CloudEmitter>();
    }

    public string subject_name = "void";

    public List<CloudEmitter> emitters;

    public void Iterate()
    {
        for(int i=0; i<emitters.Count; i++){
            emitters[i].Iterate();
        }
    }
}

public class CloudEmitter
{
    List<CloudParticle> clouds;

    public string directionMode = "radial";
    public string rotationMode = "uniform rotation";

    public int lifeSpanAverage = 230;
    public int lifeSpanRange = 60;

    public float spawnChance = 25;
    public Rect spawnRegion = new Rect(600,300,500,100);
    
    private GameObject canvas;

    private Texture2D atlas;

    public Vector2 localMaximumPoint;

    public CloudEmitter(GameObject Canvas,Texture2D cloudAtlas)
    {
        canvas = Canvas;
        atlas = cloudAtlas;
        clouds = new List<CloudParticle>();


    }

    public void Iterate()
    {
        if(Random.Range(0,100) < spawnChance){

            int cloudType = Random.Range(0,5);

            Sprite cloud = Sprite.Create(atlas,new Rect(cloudType * 512,0,512,512),new Vector2(0,0));

            float angle = Random.Range(-30f,30f);

            Vector2 spawnPos = new Vector2(Random.Range(spawnRegion.x,spawnRegion.x + spawnRegion.width),Random.Range(spawnRegion.y,spawnRegion.y + spawnRegion.height));



            CloudParticle c = new CloudParticle(canvas,cloud,spawnPos,new Vector2(Random.Range(-30,30),Random.Range(-30,30)),angle);

            Vector2 localSpawnPos = spawnPos - new Vector2(spawnRegion.x,spawnRegion.y);

            

            c.SetScale((localMaximumPoint - localSpawnPos).magnitude * -1 / 150 + 5);

            c.initialPosition = new Vector2(c.initialPosition.x,c.initialPosition.y + c.scale * 20);
            
            c.lifeSpan = lifeSpanAverage + Random.Range(-lifeSpanRange,lifeSpanRange);
            clouds.Add(c);

        }
        for(int i=0; i<clouds.Count; i++){
            if(clouds[i].complete){
                FocusScript.DestroyObject(clouds[i].self);
                clouds.RemoveAt(i);
                
            }else{
                clouds[i].Iterate();
            }
            
        }
    }



}

public class CloudParticle
{
    public bool complete = false;
    public int lifeSpan{get;set;}

    public Vector2 initialPosition;
    private Vector2 delta;

    private float initialRotation;
    private float deltaR;

    private int iterator = 0;

    public GameObject self;
    private Image im;
    private RectTransform imTransform;

    private Sprite sprite;

    public float scale;

    public void SetScale(float s){
        scale = s;
        imTransform.localScale = new Vector3(s,s,1);
    }

    
    
    public CloudParticle(GameObject canvas, Sprite s,Vector2 Pos,Vector2 deltaTransform,float deltaRotation)
    {
        initialRotation = Random.Range(0.0f,6.28f);
        deltaR = deltaRotation;
        initialPosition = Pos;
        delta = deltaTransform;
        self = new GameObject();
        self.transform.parent = canvas.transform;

        im = self.AddComponent<Image>();
        im.transform.position = Pos;

        scale = Random.Range(0.5f,2.0f);
        im.transform.localScale = new Vector3(scale,scale,1);
        
        sprite = s;
        im.sprite = sprite;

        imTransform = im.rectTransform;

    }

    public void Iterate()
    {
        if(iterator>lifeSpan){
            complete = true;
        }
        float progress = (float)iterator / (float)lifeSpan;
        float transparency = 0.5f * Mathf.Sin(2f * Mathf.PI * (progress - 0.25f) ) + 0.5f;
        /*
        Texture2D tex = sprite.texture;
        
        for(int y=0; y<tex.height; y++){
            for(int x=0; x<tex.width; x++){
                Color col = tex.GetPixel(x,y);
                col.a = transparency;
                tex.SetPixel(x,y,col);
            }
        }
        /*
        tex.Apply();
        sprite = Sprite.Create(tex,new Rect(0,0,512,512),new Vector2(0,0));*/
        //renderer.color = new Color(1,1,0,transparency);
        im.color = new Color(1,1,1,transparency);
        Vector2 tmp = initialPosition + progress * delta;
        
        imTransform.position = new Vector3(tmp.x,tmp.y,0);
        imTransform.Rotate(new Vector3(0,0,(float)deltaR/(float)lifeSpan));
        


        iterator++;
    }




}


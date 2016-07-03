import org.openkinect.freenect.*;
import org.openkinect.processing.*;

Kinect kinect;
float[] depthLookUp = new float[2048];
float factor = 300;
float exportFactor = 0.05;
int step = 2;
PVector adjust;
float rotateY;
float rotateSpeed = 1;
int mode = 0;//0:scaling,1:cubetransformControl,2:rotate

Cube area;
Config config;

JSONObject json;
String outFileName = "temp.json";
boolean isKeyPressed;

PointCloudHandler pointCloud;
void setup() {
  
  size(800, 600, P3D);
  kinect = new Kinect(this);
  kinect.initDepth();
  kinect.initVideo();
  adjust =  new PVector(width/2, height/2, -50);
  for (int i = 0; i < depthLookUp.length; i++) {
    depthLookUp[i] = rawDepthToMeters(i);
  }
  
  config = new Config();
  config.load();
  if(config.successLoad){
    area = new Cube(config.areaPos, config.areaScale,color(255, 0, 0, 127));
  }else{
    area = new Cube(new PVector(adjust.x, adjust.y, adjust.z), new PVector(100, 100, 100), color(255, 0, 0, 127));
  }
  
  pointCloud = new PointCloudHandler();
}

void update() {
  if (keyPressed) {
    if(!isKeyPressed){
      if(key == ENTER){
        export();
      }else if(key == ' '){
        exportConfig();
      }
    }
    isKeyPressed = true;
    
    if (key == CODED) {
      if (keyCode == RIGHT) {
        rotateY++;
      } else if (keyCode == LEFT) {
        rotateY--;
      } else if (keyCode == UP) {
        factor++;
      } else if (keyCode == DOWN) {
        factor--;
      }
    } else {
      if (key == 'q') {
        area.scale.x--;
      } else if (key == 'w') {
        area.scale.x++;
      } else if (key == 'e') {
        area.scale.y--;
      } else if (key == 'r') {
        area.scale.y++;
      } else if (key == 't') {
        area.scale.z--;
      } else if (key == 'y') {
        area.scale.z++;
      } else if (key == 'f') {
        area.pos.z++;
      } else if (key == 'v') {
        area.pos.z--;
      }
      if (key == 'c') {
        area.pos.x++;
      } else if (key == 'z') {
        area.pos.x--;
      } else if (key == 's') {
        area.pos.y++;
      } else if (key == 'x') {
        area.pos.y--;
      }
    }
  }else{
   isKeyPressed = false; 
  }
}

void draw() {
  update();

  background(0);
  pushMatrix();
  translate(adjust.x, adjust.y, adjust.z);
  rotateY(radians(rotateY));
  translate(-adjust.x, -adjust.y, -adjust.z);
  int[] depth = kinect.getRawDepth();
  ArrayList<Point> points = new ArrayList<Point>();

  for (int x = 0; x < kinect.width; x += step) {
    for (int y = 0; y < kinect.height; y += step) {
      int offset = x + y*kinect.width;
      int rawDepth = depth[offset];
      PVector v = depthToWorld(x, y, rawDepth);
      PVector pos = new PVector(v.x*factor + adjust.x, v.y*factor + adjust.y, factor-v.z*factor + adjust.z);
      points.add(new Point(pos, color(255)));
    }
  }
  pointCloud.setPoints(points);
  pointCloud.draw();
  area.draw();
  popMatrix();
  drawInfo();
}

float rawDepthToMeters(int depthValue) {
  if (depthValue < 2047) {
    return (float)(1.0 / ((double)(depthValue) * -0.0030711016 + 3.3309495161));
  }
  return 0.0f;
}

PVector depthToWorld(int x, int y, int depthValue) {

  final double fx_d = 1.0 / 5.9421434211923247e+02;
  final double fy_d = 1.0 / 5.9104053696870778e+02;
  final double cx_d = 3.3930780975300314e+02;
  final double cy_d = 2.4273913761751615e+02;

  PVector result = new PVector();
  double depth =  depthLookUp[depthValue];//rawDepthToMeters(depthValue);
  result.x = (float)((x - cx_d) * depth * fx_d);
  result.y = (float)((y - cy_d) * depth * fy_d);
  result.z = (float)(depth);
  return result;
}

void drawInfo() {
  rectMode(CORNER);
  fill(255, 100);
  noStroke();
  rect(0, 0, width/4, (width/4));
  textAlign(LEFT, TOP);
  float fontSize = 12;
  float margin = fontSize/2;
  textSize(fontSize);
  fill(255, 0, 0);
  text("scalingFactor : " + factor, 0, (fontSize + margin)*0);
  //text("areaScaleX : " + factor, 0, (fontSize + margin)*1);
}

void keyPressed() {
  
}

void exportConfig(){
  config.export();
  println("complete export configFile");
}

void export() {
  json = new JSONObject();
  json.setJSONArray("data",pointCloud.getData());
  saveJSONObject(json, "data/new.json");
  println("complete export");
}

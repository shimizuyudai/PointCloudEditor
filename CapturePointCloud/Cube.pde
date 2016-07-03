class Cube {
  PVector pos;
  PVector scale;
  color col;
  Cube(PVector pos,PVector scale,color col) {
    this.pos = pos;
    this.scale = scale;
    this.col = col;
  }

  void draw() {
    
    rectMode(CENTER);
    fill(col);
    stroke(255);
    pushMatrix();
    translate(pos.x, pos.y, pos.z);
    
    pushMatrix();
    translate(scale.x/2, 0, 0);
    rotateY(radians(90));
    rect(0, 0, scale.z, scale.y);
    popMatrix();
    
    pushMatrix();
    translate(-scale.x/2, 0, 0);
    rotateY(radians(-90));
    rect(0, 0, scale.z, scale.y);
    popMatrix();
    
    pushMatrix();
    translate(0, 0, scale.z/2);
    rect(0, 0, scale.x, scale.y);
    popMatrix();
    
    pushMatrix();
    translate(0, 0, -scale.z/2);
    rect(0, 0, scale.x, scale.y);
    popMatrix();
    
    pushMatrix();
    translate(0, scale.y/2, 0);
    rotateX(radians(90));
    rect(0, 0, scale.x, scale.z);
    popMatrix();
    
    pushMatrix();
    translate(0, scale.y/2, 0);
    rotateX(radians(-90));
    rect(0, 0, scale.x, scale.z);
    popMatrix();
    
    popMatrix();
  }
}

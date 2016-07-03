class PointCloudHandler {
  ArrayList<Point> points;
  PointCloudHandler() {
  }
  void setPoints(ArrayList<Point> points) {
    this.points = points;
  }

  JSONArray getData() {
    JSONArray result = new JSONArray();
    for (int i = 0; i < points.size (); i++) {
      if (points.get(i).pos.x > area.pos.x - area.scale.x/2 && points.get(i).pos.x < area.pos.x + area.scale.x/2) {
        if (points.get(i).pos.y > area.pos.y - area.scale.y/2 && points.get(i).pos.y < area.pos.y + area.scale.y/2) {
          if (points.get(i).pos.z > area.pos.z - area.scale.z/2 && points.get(i).pos.z < area.pos.z + area.scale.y/2) {
            JSONArray pointData = new JSONArray();
            PVector pos = new PVector((points.get(i).pos.x - adjust.x)*exportFactor,(points.get(i).pos.y - adjust.y)*exportFactor,(points.get(i).pos.z - adjust.z)*exportFactor);
            pointData.append(pos.x);
            pointData.append(-pos.y);
            pointData.append(pos.z);
            result.append(pointData);
          }
        }
      }
    }
    return result;
  }

  void draw() {
    stroke(255);
    for (int i = 0; i < points.size (); i++) {
      stroke(points.get(i).col);
      point(points.get(i).pos.x, points.get(i).pos.y, points.get(i).pos.z);
    }
  }
}

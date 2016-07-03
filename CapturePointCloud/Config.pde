class Config {
  String fileName = "data/config.json";
  PVector areaPos;
  PVector areaScale;
  float scalingFactor;
  String areaPosLabel = "areaPos";
  String areaScaleLabel = "areaScale";
  String scalingFactorLabel = "scalingFactor";

  boolean successLoad;

  Config() {
  }

  void load() {
    try {
      JSONObject json = loadJSONObject(fileName);
      this.scalingFactor = json.getFloat(scalingFactorLabel);
      JSONArray areaScaleData = json.getJSONArray(areaScaleLabel);
      JSONArray areaPosData = json.getJSONArray(areaPosLabel);
      this.areaScale = new PVector(areaScaleData.getFloat(0), areaScaleData.getFloat(1), areaScaleData.getFloat(2));
      this.areaPos= new PVector(areaPosData.getFloat(0), areaPosData.getFloat(1), areaPosData.getFloat(2));
      successLoad = true;
    }
    catch(Exception e) {
      successLoad = false;
    }
  }

  void export() {
    JSONObject result = new JSONObject();
    JSONArray areaPosData = new JSONArray();
    areaPosData.append(area.pos.x);
    areaPosData.append(area.pos.y);
    areaPosData.append(area.pos.z);

    JSONArray areaScaleData = new JSONArray();
    areaScaleData.append(area.scale.x);
    areaScaleData.append(area.scale.y);
    areaScaleData.append(area.scale.z);

    result.setJSONArray(areaPosLabel, areaPosData);
    result.setJSONArray(areaScaleLabel, areaScaleData);
    result.setFloat(scalingFactorLabel, scalingFactor);

    saveJSONObject(result, fileName);
  }
}

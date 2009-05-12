function MapTypeControl(opt_opts) {
  this.options = opt_opts || {};
}

MapTypeControl.prototype = new GControl();

MapTypeControl.prototype.initialize = function(map) {
  var container = document.createElement("div");
  var me = this;
  var mapDiv = me.createButton_("Map");
  var satDiv = me.createButton_("Satellite");
  var hybDiv = me.createButton_("Hybrid");
  var phyDiv = me.createButton_("Terrain");
 
  me.assignButtonEvent_(mapDiv, map, G_NORMAL_MAP, [phyDiv, satDiv, hybDiv]);
  me.assignButtonEvent_(phyDiv, map, G_PHYSICAL_MAP, [mapDiv, satDiv, hybDiv]);
  me.assignButtonEvent_(satDiv, map, G_SATELLITE_MAP, [phyDiv, mapDiv, hybDiv]);
  me.assignButtonEvent_(hybDiv, map, G_HYBRID_MAP, [phyDiv, satDiv, mapDiv]);
  GEvent.addListener(map, "maptypechanged", function() {
    if (map.getCurrentMapType() == G_NORMAL_MAP) {
      GEvent.trigger(mapDiv, "click"); 
    } else if (map.getCurrentMapType() == G_PHYSICAL_MAP) {
      GEvent.trigger(phyDiv, "click");
    } else if (map.getCurrentMapType() == G_SATELLITE_MAP) {
      GEvent.trigger(satDiv, "click");
    } else if (map.getCurrentMapType() == G_HYBRID_MAP) {
      GEvent.trigger(hybDiv, "click");
    }
  });

  container.appendChild(mapDiv);
  container.appendChild(satDiv);
  container.appendChild(hybDiv);
  container.appendChild(phyDiv);

  map.getContainer().appendChild(container);

  GEvent.trigger(map, "maptypechanged");
  return container;
}

MapTypeControl.prototype.createButton_ = function(text) {
  var buttonDiv = document.createElement("div");
  this.setButtonStyle_(buttonDiv);
  buttonDiv.style.cssFloat = "left";
  buttonDiv.style.styleFloat = "left";
  var textDiv = document.createElement("div");
  textDiv.appendChild(document.createTextNode(text));
  textDiv.style.width = "6em";
  buttonDiv.appendChild(textDiv);
  return buttonDiv;
}

MapTypeControl.prototype.assignButtonEvent_ = function(div, map, mapType, otherDivs) {
  var me = this;

  GEvent.addDomListener(div, "click", function() {
    for (var i = 0; i < otherDivs.length; i++) {
      me.toggleButton_(otherDivs[i].firstChild, false);
    }
    me.toggleButton_(div.firstChild, true);
    map.setMapType(mapType);
  });
}

MapTypeControl.prototype.toggleButton_ = function(div, boolCheck) {
   div.style.fontWeight = boolCheck ? "bold" : "";
   div.style.border = "1px solid white";
   var shadows = boolCheck ? ["Top", "Left"] : ["Bottom", "Right"];
   for (var j = 0; j < shadows.length; j++) {
     div.style["border" + shadows[j]] = "1px solid #b0b0b0";
  } 
}

MapTypeControl.prototype.getDefaultPosition = function() {
  return new GControlPosition(G_ANCHOR_TOP_RIGHT, new GSize(7, 7));
}

MapTypeControl.prototype.setButtonStyle_ = function(button) {
  button.style.color = "#000000";
  button.style.backgroundColor = "white";
  button.style.font = "small Arial";
  button.style.border = "1px solid black";
  button.style.padding = "0px";
  button.style.margin= "0px";
  button.style.textAlign = "center";
  button.style.fontSize = "12px"; 
  button.style.cursor = "pointer";
}


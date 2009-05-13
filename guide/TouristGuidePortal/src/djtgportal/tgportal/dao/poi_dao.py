from djtgportal.tgportal.models import POI


class PoiDao():
    
    def __init__(self):
        """ """
    
    def find_by_area_and_lang(self, top_left_lat, top_left_lon,
                         bottom_right_lat, bottom_right_lon, language):
        """ Get list of pois by specified area (four coordinates). """
        
        print "%s,%s,%s,%s %s" % (top_left_lat, top_left_lon,
                         bottom_right_lat, bottom_right_lon, language)
        poi_list = POI.objects.filter(lang=language, 
                                      latitude__lte=top_left_lat,
                                      longitude__gte=top_left_lon,
                                      latitude__gte=bottom_right_lat,
                                      longitude__lte=bottom_right_lon)
        return poi_list

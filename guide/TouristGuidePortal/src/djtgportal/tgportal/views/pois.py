from django.views.decorators.http import require_GET
from django.http import HttpResponse
from django.http import HttpResponseBadRequest
from django.template import Context, loader
#from django.shortcuts import render_to_response
from djtgportal.settings import MEDIA_URL
from djtgportal.tgportal.dao.lang_dao import LangDao
from djtgportal.tgportal.dao.poi_dao import PoiDao

@require_GET
def list(request, area, lang_shortcut):
    # process area parameter
    try:
        area_array = area.split(',')
        top_left_latitude = float(area_array[0])
        top_left_longitude = float(area_array[1])
        bottom_right_latitude = float(area_array[2])
        bottom_right_longitude = float(area_array[3]) 
    except Exception, e:
        return HttpResponseBadRequest('<?xml version="1.0" encoding="utf-8"?>' +
                                      '<error>Bad area parameter.</error>',
                                      content_type='text/xml')
    # get pois list
    lang_dao = LangDao()
    poi_dao = PoiDao()
    lang = lang_dao.get_by_shortcut(lang_shortcut)
    poi_list = poi_dao.find_by_area_and_lang(top_left_latitude, top_left_longitude,
                           bottom_right_latitude, bottom_right_longitude, lang)
    # render view
    t = loader.get_template('tgportalws/pois_list.xml')
    c = Context({'poi_list': poi_list, 'media_url': MEDIA_URL,})
    return HttpResponse(t.render(c), content_type='text/xml')

#def post_only(func):
#    def decorated(request, *args, **kwargs):
#        if request.method != 'POST':
#            return HttpResponseNotAllowed('Only POST here')
#        return func(request, *args, **kwargs)
#    return decorated
#
#def get_only(func):
#    def decorated(request, *args, **kwargs):
#        if request.method != 'GET':
#            return HttpResponseNotAllowed('Only GET here')
#        return func(request, *args, **kwargs)
#    return decorated
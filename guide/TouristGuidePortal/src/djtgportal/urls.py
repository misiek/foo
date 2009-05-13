from django.conf.urls.defaults import *

# Uncomment the next two lines to enable the admin:
from django.contrib import admin
admin.autodiscover()

urlpatterns = patterns('',
    # Example:
    # (r'^djtgportal/', include('djtgportal.foo.urls')),

    # Uncomment the admin/doc line below and add 'django.contrib.admindocs' 
    # to INSTALLED_APPS to enable admin documentation:
    # (r'^admin/doc/', include('django.contrib.admindocs.urls')),

    # Uncomment the next line to enable the admin:
#    (r'^tgportalws/pois/area/(?P<area>[\d.,]+)/$', 'djtgportal.tgportal.views.pois.list'),
    (r'^tgportalws/pois/area/(?P<area>.+)/lang/(?P<lang_shortcut>.+)/$', 'djtgportal.tgportal.views.pois.list'),
    (r'^admin/(.*)', admin.site.root),
)

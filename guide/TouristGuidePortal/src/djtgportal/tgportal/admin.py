from djtgportal.tgportal.models import Type
from djtgportal.tgportal.models import POI
from djtgportal.tgportal.models import MediaFile
from djtgportal.tgportal.models import MainDetail
from djtgportal.tgportal.models import SubDetail

from django.contrib import admin

admin.site.register(Type)
admin.site.register(MediaFile)

class SubDetailInline(admin.TabularInline):
    model = SubDetail
    extra = 2

class MainDetailAdmin(admin.ModelAdmin):
    inlines = [SubDetailInline]

admin.site.register(MainDetail, MainDetailAdmin)


class MainDetailInline(admin.TabularInline):
    model = MainDetail
    extra = 1

class POIAdmin(admin.ModelAdmin):
    inlines = [MainDetailInline]

admin.site.register(POI, POIAdmin)

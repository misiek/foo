from django.db import models


class MediaFile(models.Model):
    """ MediaFile class represents media associated with POI or Detail.
        (image, mp3, avi etc)
    """
    title = models.CharField(max_length=200)
    descr = models.TextField()
    file = models.FileField(upload_to='media_files')
    
    def __unicode__(self):
        return self.title


class Type(models.Model):
    """ Type class represents type of POI.
        (monument, cinema, restaurant etc)
    """
    name = models.CharField(max_length=200)
    descr = models.TextField()
    
    def __unicode__(self):
        return self.name

class POI(models.Model):
    """ Represents Point Of Interest - geografical point on map.
        (monument, cinema, restaurant etc)
    """
    latitude = models.FloatField()
    longitude = models.FloatField()
    type = models.ForeignKey(Type)
    name = models.CharField(max_length=200)
    descr = models.TextField()
    media_files = models.ManyToManyField(MediaFile, blank=True)
    
    def __unicode__(self):
        return self.name

class Detail(models.Model):
    """ Detail abstract class """
    class Meta:
        abstract = True
    title = models.CharField(max_length=200)
    descr = models.TextField()
    media_files = models.ManyToManyField(MediaFile, blank=True)
    
    def __unicode__(self):
        return self.title

class MainDetail(Detail):
    """ MainDetail class represents detail of POI.
        (text sections, tabs etc)
    """
    poi = models.ForeignKey(POI)

class SubDetail(Detail):
    """ SubDetail class represents detail of POI's MainDetail.
        (sub sections, sections in tabs etc)
    """
    main_detail = models.ForeignKey(MainDetail)

from django.db import models


class MediaFile(models.Model):
    """ MediaFile class represents media associated with POI or Detail.
        (image, mp3, avi etc)
    """
    title = models.CharField(max_length=200, help_text="Title of file.")
    descr = models.TextField(help_text="Description of file.")
    file = models.FileField(upload_to='media_files', help_text="Media file.")
    
    def __unicode__(self):
        return self.title


class Type(models.Model):
    """ Type class represents type of POI.
        (monument, cinema, restaurant etc)
    """
    name = models.CharField(max_length=200, help_text="Defines the type.")
    descr = models.TextField(help_text="Type description.")
    
    def __unicode__(self):
        return self.name

class POI(models.Model):
    """ Represents Point Of Interest - geografical point on map.
        (monument, cinema, restaurant etc)
    """
    latitude = models.FloatField(help_text="Latitude of a point.")
    longitude = models.FloatField(help_text="Longitude of a point.")
    type = models.ForeignKey(Type, help_text="Type of a point.")
    name = models.CharField(max_length=200, help_text="Name of a point.")
    descr = models.TextField(help_text="Point description.")
    media_files = models.ManyToManyField(MediaFile, blank=True,
                            help_text="Media files associated with a point.")
    
    def __unicode__(self):
        return self.name

class Detail(models.Model):
    """ Detail abstract class """
    class Meta:
        abstract = True
    title = models.CharField(max_length=200, help_text="Title of a detail.")
    descr = models.TextField(help_text="Description of a detail.")
    media_files = models.ManyToManyField(MediaFile, blank=True,
                            help_text="Media files associated with a detail.")
    
    def __unicode__(self):
        return self.title

class MainDetail(Detail):
    """ MainDetail class represents detail of POI.
        (text sections, tabs etc)
    """
    poi = models.ForeignKey(POI, help_text="POI characterized by a detail.")

class SubDetail(Detail):
    """ SubDetail class represents detail of POI's MainDetail.
        (sub sections, sections in tabs etc)
    """
    main_detail = models.ForeignKey(MainDetail,help_text=
                                    "MainDetail characterized by a detail.")
from django.db import models


class MediaFile(models.Model):
    """ Media file associated with POI or Detail.
        It can be: image, mp3, avi etc.
    """
    title = models.CharField(max_length=200)
    description = models.TextField()
    file = models.FileField(upload_to='media_files')

class Type(models.Model):
    """ POI type - monument, cinema, restaurant etc. """
    name = models.CharField(max_length=200)
    description = models.TextField()

class Detail(models.Model):
    """ POI detail - it can be used to give more detail information about POIs
        (text sections, tabs etc).
    """
    title = models.CharField(max_length=200)
    description = models.TextField()
    media_files = models.ManyToManyField(MediaFile)
    # each detail can have its own details
    datails = models.ForeignKey('self')

class POI(models.Model):
    """ Represents Point Of Interest - geografical point on map.
        It can be: monument, cinema, restaurant etc.
    """
    latitude = models.FloatField()
    longitude = models.FloatField()
    type = models.ForeignKey(Type)
    name = models.CharField(max_length=200)
    description = models.TextField()
    media_files = models.ManyToManyField(MediaFile)
    datails = models.ManyToManyField(Detail)

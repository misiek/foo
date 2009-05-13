from djtgportal.tgportal.models import Lang


class LangDao():
    
    def __init__(self):
        """ """
    
    def get_by_shortcut(self, shortcut):
        """ Get language object by shortcut. """
        return Lang.objects.get(lang=shortcut)

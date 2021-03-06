#!/usr/bin/python

import urllib

class MapDownloader():
    ''' Download map from http://www.openstreetmap.org
    '''

    def __init__(self):
        ''' Initialize area coordinates, set deltas for lon and lat.
            These values are calculated to get image size close
            to 1024x1024 (in practise: 1026x1027, 1026x1026 etc).
        '''
        # left top corner of area to download
        self.x1 = 19.8669
        self.y1 = 50.1046
        # right bottom corner of area to download
        self.x2 = 20.0029
        self.y2 = 50.0233
        self.grid = []
        self.osmarender_format = 'png'
        self.osmarender_zoom = '17'
    
    def create_grid(self):
        ''' Creates coordinates grid - it is only possible to download
            big area in parts.
        '''
        i = 0
        y = self.y1
        while(y > self.y2):
            y_tmp = y
            y -= self.dy
            x = self.x1
            while(x < self.x2):
                x_tmp = x
                x += self.dx
                self.grid.append({'x1': x_tmp,
                                  'y1': y_tmp,
                                  'x2': x,
                                  'y2': y})
                i += 1
        print "Total:", i

    def get_map(self, minlon, maxlat, maxlon, minlat, dir=None):
        ''' Downloads map image and saves as file.
            self.get_map('19.9439', '50.1046', '19.9549', '50.0975')
        '''
        url = 'http://www.openstreetmap.org/export/finish'
        params = {
            'minlon': minlon,
            'minlat': minlat,
            'maxlon': maxlon,
            'maxlat': maxlat,
            'format': 'osmarender',
            'mapnik_format': 'png',
            'mapnik_scale': '6950000',
            'osmarender_format': self.osmarender_format,
            'osmarender_zoom': self.osmarender_zoom,
            'commit': 'Export',
        }
        # file name as top left coordinates and bottom right
        name = '%s-%s_%s-%s.%s' % (minlon, maxlat, maxlon, minlat,
                                        params['osmarender_format'])
        if not dir:
            dir = '%s_%s_%s' % (params['format'],
                                params['osmarender_format'],
                                params['osmarender_zoom'])
        p = urllib.urlencode(params)
        response = urllib.urlretrieve(url, filename='%s/%s'\
                                        % (dir, name), data=p)
    
    def download(self):
        ''' Downloads all parts.
        '''
        # lon delta
        #self.dx = 0.011015
        self.dx = 0.011014 + (17 - int(self.osmarender_zoom)) * 0.011014
        # lat delta
        #self.dy = 0.007077
        self.dy = 0.007076 + (17 - int(self.osmarender_zoom)) * 0.007076
        self.create_grid()
        i = 1
        for p in self.grid:
            print '%d part: 1[%s, %s] 2[%s, %s]' %\
                        (i, p['x1'], p['y1'], p['x2'], p['y2'])
            self.get_map(p['x1'], p['y1'], p['x2'], p['y2'])
            i += 1
        

if __name__ == '__main__':
    mapd = MapDownloader()
    # left top corner of area to download
    mapd.x1 = 19.8669
    mapd.y1 = 50.1046
    # right bottom corner of area to download
    mapd.x2 = 20.0029
    mapd.y2 = 50.0233
    mapd.osmarender_format = 'jpeg'
    mapd.osmarender_zoom = '16'
    mapd.download()

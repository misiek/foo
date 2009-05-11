#!/usr/bin/python

import os
import sys
import Image

class MapPackageMaker():
    ''' Creates map package - map image is being cut to parts which size
        is arround 256x256.
        Config file is being generated (map.xml)
    '''

    def __init__(self, image_path, dst_dir):
        # load map image file
        self.map_image = Image.open(image_path)
        # destination directory for package
        self.dst_dir = dst_dir
        # preferred edge size 
        self.preferred_edge = 256
    
    def make(self):
        ''' Creates map package.
        '''
        # get image format
        self.map_image_format = self.map_image.format.lower()
        # get package name
        file_name = os.path.basename(self.map_image.filename)
        self.package_name = file_name.replace('.'+self.map_image_format, '')
        #print self.package_name
        # create package dir
        self.package_dir = dst_dir + '/' + self.package_name
        if not os.path.isdir(self.package_dir):
            os.mkdir(self.package_dir)
        # create sub images dir
        self.parts_dir = self.package_dir + '/parts'
        if not os.path.isdir(self.parts_dir):
            os.mkdir(self.parts_dir)
        self.__save_parts()
        self.__save_map_xml()
        
    def __save_map_xml(self):
        ''' Saves map xml config (map.xml).
        '''
        # get coordinates from package name
        coordinates = self.package_name.split('_');
        top_left = coordinates[0].split('-');
        bottom_right = coordinates[1].split('-');
        # build xml config
        xml = []
        xml.append('<?xml version="1.0" encoding="UTF-8"?>')
        xml.append('<map>')
        xml.append('<coordinates>')
        xml.append('<topLeft>')
        xml.append('<longitude>')
        xml.append(top_left[0])
        xml.append('</longitude>')
        xml.append('<latitude>')
        xml.append(top_left[1])
        xml.append('</latitude>')
        xml.append('</topLeft>')
        xml.append('<bottomRight>')
        xml.append('<longitude>')
        xml.append(bottom_right[0])
        xml.append('</longitude>')
        xml.append('<latitude>')
        xml.append(bottom_right[1])
        xml.append('</latitude>')
        xml.append('</bottomRight>')
        xml.append('</coordinates>')
        xml.append('<parts>')
        xml.append('<format>')
        xml.append(self.map_image_format)
        xml.append('</format>')
        xml.append('</parts>')
        xml.append('</map>')
        # write config xml to file
        map_xml = open(self.package_dir + '/map.xml', 'w')
        map_xml.write(''.join(xml))
        map_xml.write("\n")
        map_xml.close()

    def __save_parts(self):
        ''' Cuts out and saves sub images.
        '''
        # get source image size
        width, height = self.map_image.size 
        #print "Width=%s Height=%s" % (width, height)
        # misfit values 
        misfit_width = width % self.preferred_edge
        misfit_height = height % self.preferred_edge
        #print "Misfit_Width=%s Misfit_Height=%s" % (misfit_width, misfit_height)
        # preferred edge count in size 
        count_width = width / self.preferred_edge
        count_height = height / self.preferred_edge
        #print "Count_Width=%s Count_Height=%s" % (count_width, count_height)
        # when it is "big" put misfit as separate image
        # in width dimension
        if misfit_width > self.preferred_edge/4:
            count_width += 1
        # when it is "big" put misfit as separate image
        # in height dimension
        if misfit_height > self.preferred_edge/4:
            count_height += 1
        # process sub images
        for h in range(count_height):
            for w in range(count_width):
                w1 = w * self.preferred_edge
                h1 = h * self.preferred_edge
                w2 = (w+1) * self.preferred_edge
                # when processing last sub image make sure to go 
                # to the end - width dimension
                if w+1 == count_width:
                    w2 = width
                h2 = (h+1) * self.preferred_edge
                # when processing last sub image make sure to go
                # to the end - width dimension
                if h+1 == count_height:
                    h2 = height
                # coordinates for cutting sub image
                sub_image_box = (w1, h1, w2, h2)
                # cut out sub image
                sub_image = self.map_image.crop(sub_image_box)
                # prepare sub image file name
                sub_image_name = "%d:%d.%s" % (h, w, self.map_image_format)
                # save sub image to file
                sub_image.save(self.parts_dir + "/" + sub_image_name)
                #print "1[%s, %s] 2[%s, %s]" % sub_image_box
                #print "image '%s'" % sub_image_name



class DirMapPackageMaker():
    ''' Creates map packages from each map image file in given directory.
    '''
    
    def __init__(self, src_dir, dst_dir):
        # source directory
        self.src_dir = src_dir
        # destination directory
        self.dst_dir = dst_dir
    
    def make(self):
        # read images to be packaged
        map_images = os.listdir(src_dir)
        print "Total: ", len(map_images)
        # prepare destination directory
        if not os.path.isdir(dst_dir):
            os.mkdir(dst_dir)
        i = 1
        for image in map_images:
            image_path = src_dir + '/' + image
            print '%d Packaging "%s"' % (i, image_path)
            MapPackageMaker(image_path, dst_dir).make()
            i += 1


if __name__ == '__main__':
    src_dir = sys.argv[1]
    l = src_dir.split('_')
    img_type = l[1]
    zoom = l[2]
    dst_dir = './map_pkgs/' + img_type + '/zoom_' + zoom
    dmpm = DirMapPackageMaker(src_dir, dst_dir)
    dmpm.make()

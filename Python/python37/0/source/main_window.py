#!/usr/bin/env python3
# -*- coding: utf-8 -*-

#import resources.system_resources

from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *

from source.image_button import *

class MainWindow(QMainWindow):
 

    def __init__(self):
        super().__init__()

    def create_main_window_layout(self):

        pixel_ratio = QWindow().devicePixelRatio()

        #set QWidget class
        self.main_window_widget = QWidget()

        #set background picture by QLabel
        main_background_pixmap = QPixmap('resources/main_background.png')

        main_background_pixmap.setDevicePixelRatio(pixel_ratio)
        self.main_background = QLabel(self.main_window_widget)
        self.main_background.setPixmap(main_background_pixmap)
        self.main_background.setGeometry(0, 0, 640, 480)

 
        #create a strat button
        self.main_play_button = ImageButton('main_start', self.main_window_widget)
        self.main_play_button.setGeometry(0, 400, 300, 32)

        #create a multiplay button
        self.main_multiplay_button = ImageButton('main_multiplay', self.main_window_widget)
        self.main_multiplay_button.setGeometry(150, 400, 300, 32)
		
        #create a multiplay local button
        self.main_multiplay2_button = ImageButton('main_2players', self.main_window_widget)
        self.main_multiplay2_button.setGeometry(150, 300, 300, 32)	
        self.main_multiplay2_button.hide()	
		
        #create a multiplay network button
        self.main_multiplaynetwork_button = ImageButton('main_network', self.main_window_widget)
        self.main_multiplaynetwork_button.setGeometry(150, 350, 300, 32)	
        self.main_multiplaynetwork_button.hide()			

        #create a score button
        self.main_score_button = ImageButton('main_score', self.main_window_widget)
        self.main_score_button.setGeometry(300, 400, 300, 32)


        #create a quit button
        self.main_quit_button = ImageButton('main_exit', self.main_window_widget)
        self.main_quit_button.setGeometry(500, 400, 300, 32)
		


	

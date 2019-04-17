import sys
import time

from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *

 
class ImageAnimation(object):



    def __init__(self, image_path, sprite_width, sprite_height, parent):
	
        pixmap = QPixmap(image_path)

        width, height = pixmap.width(), pixmap.height()
        self.pixmaps = []
        for x in range(0, width, sprite_width):
            for y in range(0, height, sprite_height):
                self.pixmaps.append(pixmap.copy(x, y, sprite_width, sprite_height))
				
        self._current_frame = 0
        self._current_start = 0			
        self._current_Count = len(self.pixmaps)		
        self.label = parent

    def play(self, interval=100):
        self._timer = QTimer(interval=interval, timeout=self._animation_step)
        self._timer.start()
		
    def _animation_step(self):
        self.label.setPixmap(self.pixmaps[self._current_frame])
        self.label.update()
        self._current_frame += 1
        if self._current_frame >= self._current_Count or self._current_frame <= self._current_start:
            self._current_frame = self._current_start
 			
      			
    def _update(self,x,y): 	
        self._current_start	= x	
        self._current_Count = y	
        
        if self._current_frame >= y and self._current_frame <= x:	
           self._current_frame = x	
		   
 			   
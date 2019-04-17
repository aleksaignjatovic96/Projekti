import random
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *

from source.image_animation import *
from source.Bullet import *

class Enemy(QLabel):
    
    x = 0
    y = 0
    width = 16
    height = 22	
    death = False


 
    def __init__(self, x, y, pozicija, parent):
        super().__init__(parent)
		
		
        self.x = x
        self.y = y				
	
        self.setGeometry(self.x, self.y, self.width, self.height)		
        
        self.animacija = ImageAnimation('resources/robot.png', self.width, self.height, self)
        self.animacija._update(0,14)		
        self.animacija.play()	

        self.steps = 0
        self.xd = 0
        self.yd = 0
        self.bulletEnemy = None
        self.show()
        self.enemy_position = pozicija

    def _move(self):
        self.x += self.xd
        self.y += self.yd		
        self.setGeometry(self.x, self.y, self.width, self.height)	
			
    def _collide(self,walls):
	
	    #Player restangle
        rect1 = [self.x, self.y, self.x+self.width, self.y+self.height] 
	
        for item in walls:

			#Wall restangle
            rect2 = [item.x-5, item.y-5, item.width+5, item.height+5]
			
            if rect1[0] < rect2[2] and rect1[2] > rect2[0] and rect1[1] < rect2[3] and rect1[3]  > rect2[1]:
                			
                return True					

    def _death(self):	
        self.death = True
        self.animacija._update(15,18)	
			
    def closeEvent(self, event):
        del self.animacija	
 
    def _shoot(self, parent):
        if self.bulletEnemy == None:
            rand = random.randrange(1,8)
            if rand == 1:
              self.bulletEnemy = Bullet(self.x + 8, self.y + 11, 0, parent)

 
 
 
 
 
			
			
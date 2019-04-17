
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *

from source.image_animation import *

class Bullet(QLabel):
    
    x = 0
    y = 0
    width = 8
    height = 6	
    ispaljen = False
 
    def __init__(self, x, y, creator, parent):
        super().__init__(parent)
		
		
        self.x = x
        self.y = y				
	
        self.setGeometry(self.x, self.y, self.width, self.height)		
        
        self.animacija = ImageAnimation('resources/bullets.png', self.width, self.height, self)
        #self.animacija._update(0,1)		
        self.animacija.play()	
		
        self.xd = 0
        self.yd = 0
        self.creator = creator
        self.show()

    def _move(self, smer):

      if self.creator == 1:
          self.bonus = 2.5
      elif self.creator == 0:
          self.bonus = -1

      if self.ispaljen == False:
        self.ispaljen = True


        if smer == 0:
          self.xd = 0
          self.yd = -2.5 - self.bonus
          self.animacija._update(3,4)
        elif smer == 1:
          self.xd = 2.5 + self.bonus
          self.yd = 0
          self.animacija._update(0,1)
        elif smer == 2:
          self.xd = 0
          self.yd = 2.5 + self.bonus
          self.animacija._update(3,4)
        elif smer == 3:
          self.xd = -2.5 - self.bonus
          self.yd = 0
          self.animacija._update(0,1)

        
		
      self.x += self.xd
      self.y += self.yd		
      self.setGeometry(self.x, self.y, self.width, self.height)	
		
					
    def _collideWall(self,walls):
	
	    #Bullet rectangle
        rect1 = [self.x, self.y, self.x+self.width, self.y+self.height] 
	
        for item in walls:

			#Target rectangle
            rect2 = [item.x-5, item.y-5, item.width+5, item.height+5]
			
            if rect1[0] < rect2[2] and rect1[2] > rect2[0] and rect1[1] < rect2[3] and rect1[3]  > rect2[1]:
                			
                return True					

    def _collideEnemy(self, enemies):
	
	    #Bullet rectangle
        rect1 = [self.x, self.y, self.x+self.width, self.y+self.height] 
	
        for item in enemies:

			#Target rectangle
            rect2 = [item.x, item.y, item.x + item.width, item.y + item.height]
			
            if rect1[0] < rect2[2] and rect1[2] > rect2[0] and rect1[1] < rect2[3] and rect1[3]  > rect2[1]:
                			
                item._death()
                return True		
				
    def _collidePlayer(self, players):
	
        players = [a for a in players if a.death == False]
		
        if len(players) == 0:
           return	       
	
	    #Bullet rectangle
        rect1 = [self.x, self.y, self.x+self.width, self.y+self.height] 
	
        for item in players:

			#Target rectangle
            rect2 = [item.x, item.y, item.x + item.width, item.y + item.height]
			
            if rect1[0] < rect2[2] and rect1[2] > rect2[0] and rect1[1] < rect2[3] and rect1[3]  > rect2[1]:
                			
                item._dead()
                item.player_life -= 1
                return True

    def closeEvent(self, event):
        del self.animacija	
 


from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *

from source.image_animation import *
from source.Bullet import *

class Player(QLabel):
    
    x = 0
    y = 0
    width = 16
    height = 34	
    player_size = 15
    player_mode = "-"	
    player_life = 3	
    player_score = 0	
    death = False
    new_level_ind = False
    player_ind = None
	
    last_key = 1
 
    def __init__(self, x, y, num_player,parent):
        super().__init__(parent)
		
        self.x = x
        self.y = y			
	
        self.setGeometry(self.x, self.y, self.width, self.height)		

        if num_player == 0:
            self.animacija = ImageAnimation('resources/player.png', self.width, self.height, self)
        elif num_player == 1:
            self.animacija = ImageAnimation('resources/player2.png', self.width, self.height, self)
        self.animacija._update(0,1)
        self.animacija.play()	

        self.steps = 0
        self.xd = 0
        self.yd = 0
        self.last_move = None 	
        self.bullet = None

    def _position(self, x, y,mode): 
        self.x = x
        self.y = y	
        self.player_mode = mode		
		
    def _move(self, x, y,mode):
	  

        if self.death == True:
            return
			
        self.x += x
        self.y += y		
        self.setGeometry(self.x, self.y, self.width, self.height)	
		
        if self.player_mode != mode:
           self.player_mode = mode
		
           if self.player_mode == "L":
              self.animacija._update(5,9)	
           elif self.player_mode == "R":
              self.animacija._update(0,4)						   
           elif self.player_mode == "U" or self.player_mode == "D":
              self.animacija._update(8,10)			
           elif self.player_mode == "-":
              self.animacija._update(0,1)	

    def _collide(self,walls):
	
	    
        if self.death == True:
            return False
	
	    #Player restangle
        rect1 = [self.x, self.y, self.x+self.width, self.y+self.height] 
	
        for item in walls:

			#Wall restangle
            rect2 = [item.x-5, item.y-5, item.width+5, item.height+5]
			
            if rect1[0] < rect2[2] and rect1[2] > rect2[0] and rect1[1] < rect2[3] and rect1[3]  > rect2[1]:
                self.player_life -= 1			
                return True				

        return False

    def _collideDoor(self, doors):

        rect1 = [self.x, self.y, self.x + self.width, self.y + self.height]

        for item in doors:

            # Door restangle
            rect2 = [item.x - 5, item.y - 5, item.width + 5, item.height + 5]

            if rect1[0] < rect2[2] and rect1[2] > rect2[0] and rect1[1] < rect2[3] and rect1[3] > rect2[1]:
                return True

        return False

			
    def _collideEnemy(self, enemies):
	
        if self.death == True:
            return False
	
	    #Player restangle
        rect1 = [self.x, self.y, self.x+self.width, self.y+self.height] 
	
        for item in enemies:

			#enemy restangle
            rect2 = [item.x, item.y, item.x + item.width, item.y + item.height]
			
            if rect1[0] < rect2[2] and rect1[2] > rect2[0] and rect1[1] < rect2[3] and rect1[3]  > rect2[1]:
                self.player_life -= 1			
                return True				
				
 

        return False

    def _collideOtto(self, otto):
	
            if self.death == True:
                return False
            # Player restangle
            rect1 = [self.x, self.y, self.x + self.width, self.y + self.height]

                # otto restangle
            rect2 = [otto.x, otto.y, otto.x + otto.width, otto.y + otto.height]

            if rect1[0] < rect2[2] and rect1[2] > rect2[0] and rect1[1] < rect2[3] and rect1[3] > rect2[1]:
                self.player_life -= 1
                return True

            return False

    def _collideDeus_ex_machina(self, deus_ex_machina):

            if self.death == True:
                return False

            # Player restangle
            rect1 = [self.x, self.y, self.x + self.width, self.y + self.height]

                # deus_ex_machina restangle
            rect2 = [deus_ex_machina.x, deus_ex_machina.y, deus_ex_machina.x + deus_ex_machina.width, deus_ex_machina.y + deus_ex_machina.height]

            if rect1[0] < rect2[2] and rect1[2] > rect2[0] and rect1[1] < rect2[3] and rect1[3] > rect2[1]:

                if deus_ex_machina.active == True:
                    self.player_life += 1
                    deus_ex_machina.active = False
                    deus_ex_machina.close()
                    return True

            return False

    def closeEvent(self, event):
        del self.animacija	
 
    def _dead(self):	
        self.death = True
        self.animacija._update(10,14)

    def _newLevel(self):
        self.new_level_ind = True
        self.animacija._update(10,14)

    def _live(self):
        self.death = False
        self.animacija._update(0,1)
        self.show()
        if self.player_ind == "player1":
            self._position(30, 200, "-")
        elif self.player_ind == "player2":
            self._position(590, 200, "-")
		
    def _shoot(self, parent):
        if self.bullet == None:
            if self.last_key == 0:
                self.animacija._update(15, 16)
            elif self.last_key == 1:
                self.animacija._update(14, 15)
            elif self.last_key == 2:
                self.animacija._update(21, 22)
            elif self.last_key == 3:
                self.animacija._update(19, 20)
            self.bullet = Bullet(self.x + 8, self.y + 17, 1, parent)

 
 
 
 
 
			
			
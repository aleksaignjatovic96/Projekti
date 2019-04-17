
import random
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *

from source.wall import *
from source.enemy import *
from source.otto import *
from source.deus_ex_machina import *
 
from source.image_animation import *

class Level(QWidget):

    wall_array = [] 
    door_array = [] 	
    enemy_array = []	
    grid = {}
    otto = None
    deus_ex_machina = None
    otto_exists = False
    deus_ex_machina_exists = False
 	
    broj_polja = [0,1,3,4,6,7,8,10,11,13,14] #izbacena su vrata
	
    maze_width = 620
    maze_height = 400
    wall_width = 10
    player_size = 15	
    x0=10
    y0=10	

    def __init__(self, parent):
        super().__init__(parent)
		
        self.setGeometry(0, 0, 640, 480)
		
        self.wall_array = [] 
        self.door_array = [] 		
        self.enemy_array = []	
        self.grid = {}			
		
        self.load_grid_coord_dictionary()
		
        self.show() 	
 
 
    def  door_add(self):
	
	    #Gore
        wall = Wall(self.x0+(2*self.maze_width//5) ,self.y0-20 , self.x0+(3*self.maze_width//5),self.y0-20 ) 
        self.door_array.append(wall)

		#Dole
        wall = Wall(self.x0+(2*self.maze_width//5) ,self.y0+self.maze_height+20 , self.x0+(3*self.maze_width//5),self.y0+self.maze_height+20 ) 
        self.door_array.append(wall)	

		#Levo
        wall = Wall(self.x0-10 ,self.y0+(1*self.maze_height//3) , self.x0-10, self.y0+(2*self.maze_height//3) ) 
        self.door_array.append(wall)			

		#Desno
        wall = Wall(self.x0+self.maze_width+10 ,self.y0+(1*self.maze_height//3) , self.x0+self.maze_width+10, self.y0+(2*self.maze_height//3) ) 
        self.door_array.append(wall)			

    def  walls_add(self):
	
        wall = Wall(self.x0,self.y0 , self.x0 + (2*self.maze_width//5),self.y0)
        self.wall_array.append(wall)

        wall = Wall(self.x0+ (3*self.maze_width//5),self.y0 , self.x0+  self.maze_width, self.y0)
        self.wall_array.append(wall)
		
		#------------
		
        wall = Wall(self.x0,self.y0 , self.x0, self.y0 + (self.maze_height//3))
        self.wall_array.append(wall)
		
        wall = Wall(self.x0+self.maze_width,self.y0 , self.x0+self.maze_width,self.y0+(self.maze_height//3)) 
        self.wall_array.append(wall)

		#------------		
		
        wall = Wall(self.x0,self.y0+(2*self.maze_height//3) , self.x0,self.y0+self.maze_height ) 		
        self.wall_array.append(wall)
		
        wall = Wall(self.x0,self.y0+self.maze_height , self.x0+(2*self.maze_width//5),self.y0+self.maze_height )
        self.wall_array.append(wall)
		
		#------------
		
        wall = Wall(self.x0+self.maze_width,self.y0+(2*self.maze_height//3) , self.x0+self.maze_width,self.y0+self.maze_height ) 	
        self.wall_array.append(wall)
		
        wall = Wall(self.x0+(3*self.maze_width//5),self.y0+self.maze_height , self.x0+self.maze_width,self.y0+self.maze_height ) 	
        self.wall_array.append(wall) 
		
    def  maze_add(self):
        x0=10
        y0=10	
        maze_width = 620
        maze_height = 400	

        x = (self.maze_width//5) #128
        y = (self.maze_height//3) #133
        for i in range(1,5):
            for j in range(1,3):

                xd=self.x0 + i*x
                yd=self.y0 + j*y
                wall_dir = random.randrange(4)
                if wall_dir == 0:
                    xd = self.x0 + i*x - self.maze_width//5
                    wall = Wall(xd,y0 + j*y , x0 + i*x,yd) 
					
                if wall_dir == 1:
                    yd = self.y0 + j*y + self.maze_height//3
                    wall = Wall(x0 + i*x,y0 + j*y , xd,yd) 
					
                if wall_dir == 2:
                    xd = self.x0 + i*x + self.maze_width//5
                    wall = Wall(x0 + i*x,y0 + j*y , xd,yd) 
					
                if wall_dir == 3:
                    yd = self.y0 + j*y - self.maze_height//3
                    wall = Wall(x0 + i*x,yd , xd, y0 + j*y ) 
					
					 
                self.wall_array.append(wall)		

    def  enemy_add(self):	
	
        #add enemies to room
        broj_enemies = random.randrange(6,10)
        self.broj_polja = [0,1,3,4,6,7,8,10,11,13,14]	
		
        i = 0
        while i < broj_enemies:
		
            pozicija = random.randrange(len(self.broj_polja))
 
            self.enemy_array.append(Enemy(self.grid[self.broj_polja[pozicija]][0],self.grid[self.broj_polja[pozicija]][1], pozicija, self))
            del self.broj_polja[pozicija]
            i = i +1	

 
		
    def paintEvent(self, event):
  
        q = QPainter(self)
        q.setPen(QPen(QColor(80, 90, 220), 10));	
			
        for item in self.wall_array:
            q.drawLine(item.x, item.y , item.width, item.height) 		
 	
    def closeEvent(self, event):
	
        #brisanje enemy 
        number_enemy = len(self.enemy_array)	
        while  number_enemy != 0:
            self.enemy_array[0].close()		
            del self.enemy_array[0]
            number_enemy = len(self.enemy_array) 
			
        #brisanje zidova 
        number_walls = len(self.wall_array)	
        while  number_walls != 0:
            del self.wall_array[0]
            number_walls = len(self.wall_array) 	

        #brisanje vrata 
        number_door = len(self.door_array)	
        while  number_door != 0:
            del self.door_array[0]
            number_door = len(self.door_array) 			
	
        self.wall_array = [] 
        self.door_array = [] 		
        self.enemy_array = [] 

    def otto_create(self, player_array):

        if random.randrange(1000) == 50 and self.otto_exists == False:
            self.otto_exists = True
            pozicija = random.randrange(len(self.broj_polja))
            chase = random.randrange(2)
            self.otto = Otto(self.grid[self.broj_polja[pozicija]][0], self.grid[self.broj_polja[pozicija]][1], chase, self)
            del self.broj_polja[pozicija]

    def two_point_distance(self, en, pl):

        return ((en.x - pl.x) ** 2 + (en.y - pl.y) ** 2) ** 0.5


    def enemy_move(self,player_array):

        player_array = [a for a in player_array if a.death == False]
		
        if len(player_array) == 0:
           return	

        for item in self.enemy_array:

            (m, i) = min((self.two_point_distance(item, pnt_j), i)
                     for i, pnt_j in enumerate(player_array))

            move = random.randrange(1)

            if move == 0:
                rnd_smer = random.randrange(-1, 1)

                # follow player
                follx = item.x - player_array[i].x
                folly = item.y - player_array[i].y

                if (abs(follx) > abs(folly)):
                    if (follx > 0):
                        rnd_smer = -0.5
                    else:
                        rnd_smer = 0.5
                    item.xd = rnd_smer
                    item.yd = 0
                else:
                    if (folly > 0):
                        rnd_smer = -0.5
                    else:
                        rnd_smer = 0.5
                    item.yd = rnd_smer
                    item.xd = 0

                if item.death == False:
                    item._move()

                # Collide walls
                if item._collide(self.wall_array):
                    item._death()

            if item.death == True and item.animacija._current_frame == 17:
                idx = self.enemy_array.index(item)
                if self.enemy_array[idx].bulletEnemy != None:
                    self.enemy_array[idx].bulletEnemy.close()
                    self.enemy_array[idx].bulletEnemy = None
                del self.enemy_array[idx]
                item.close()


    def otto_move(self, player_array):
	
                player_array = [a for a in player_array if a.death == False]
		
                if len(player_array) == 0:
                   return	


                (m, i) = min((self.two_point_distance(self.otto, pnt_j), i)
                       for i, pnt_j in enumerate(player_array))

                move = random.randrange(1)

                rnd_smer = random.randrange(-1, 1)

                # follow player
                follx = self.otto.x - player_array[i].x
                folly = self.otto.y - player_array[i].y

                if (abs(follx) > abs(folly)):
                    if (follx > 0):
                        rnd_smer = -1
                    else:
                        rnd_smer = 1
                    self.otto.xd = rnd_smer
                    self.otto.yd = 0
                else:
                    if (folly > 0):
                        rnd_smer = -1
                    else:
                        rnd_smer = 1
                    self.otto.yd = rnd_smer
                    self.otto.xd = 0
                self.otto._move()

    def otto_move1(self, player_array):

        if len(player_array) == 1:

            # follow player
            follx = self.otto.x - player_array[0].x
            folly = self.otto.y - player_array[0].y

            if (abs(follx) > abs(folly)):
                if (follx > 0):
                    rnd_smer = -1
                else:
                    rnd_smer = 1
                self.otto.xd = rnd_smer
                self.otto.yd = 0
            else:
                if (folly > 0):
                    rnd_smer = -1
                else:
                    rnd_smer = 1
                self.otto.yd = rnd_smer
                self.otto.xd = 0

            self.otto._move()

        elif len(player_array) > 1:

            if self.otto.chase == 0:

                # follow player
                follx = self.otto.x - player_array[0].x
                folly = self.otto.y - player_array[0].y

                if (abs(follx) > abs(folly)):
                    if (follx > 0):
                        rnd_smer = -1
                    else:
                        rnd_smer = 1
                    self.otto.xd = rnd_smer
                    self.otto.yd = 0
                else:
                    if (folly > 0):
                        rnd_smer = -1
                    else:
                        rnd_smer = 1
                    self.otto.yd = rnd_smer
                    self.otto.xd = 0

                self.otto._move()

            elif self.otto.chase == 1:

                # follow player
                follx = self.otto.x - player_array[1].x
                folly = self.otto.y - player_array[1].y

                if (abs(follx) > abs(folly)):
                    if (follx > 0):
                        rnd_smer = -1
                    else:
                        rnd_smer = 1
                    self.otto.xd = rnd_smer
                    self.otto.yd = 0
                else:
                    if (folly > 0):
                        rnd_smer = -1
                    else:
                        rnd_smer = 1
                    self.otto.yd = rnd_smer
                    self.otto.xd = 0

                self.otto._move()

    def enemy_shoot(self):
	
       for item in self.enemy_array:
		
           item._shoot(self)
           if item.bulletEnemy != None:
              smer = random.randrange(0,3)
			  
			 
              item.bulletEnemy._move(smer)



    def enemy_bullet_collide(self, player_array):

        for item in self.enemy_array:

            if item.bulletEnemy != None:
                if item.bulletEnemy._collideWall(self.wall_array):
                    print('bullet collide wall')
                    item.bulletEnemy.close()
                    item.bulletEnemy = None

                elif item.bulletEnemy._collideWall(self.door_array):
                    print('bullet collide door')
                    item.bulletEnemy.close()
                    item.bulletEnemy = None

                elif item.bulletEnemy._collidePlayer(player_array):
                    print('bullet collide player')
                    item.bulletEnemy.close()
                    item.bulletEnemy = None
                    return True

        return False

    def _create_deus_ex_machina(self):

        if random.randrange(1500) == 100 and self.deus_ex_machina_exists == False:
            self.deus_ex_machina_exists = True
            pozicija = random.randrange(len(self.broj_polja))
            self.deus_ex_machina = DeusExMachina(self.grid[self.broj_polja[pozicija]][0], self.grid[self.broj_polja[pozicija]][1], self)
            self.deus_ex_machina.active = True
            del self.broj_polja[pozicija]

				
    def load_grid_coord_dictionary(self):
        x = (self.maze_width//5) 
        y = (self.maze_height//3)
        count = 0
        for i in range(0,3):
            for j in range(0,5):
                xp  =  self.x0 + j*x + self.wall_width - self.player_size//2
                yp  =  self.y0 + i*y - self.wall_width + self.player_size//2
 
                self.grid.update({count: [xp + x//2,yp+y//2]})
                count = count + 1	
	
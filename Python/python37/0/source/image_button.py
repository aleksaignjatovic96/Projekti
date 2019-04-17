#import resources.system_resources

from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *

class ImageButton(QAbstractButton):

 
    menu_button = 0
    menu_focused = None

    def __init__(self, button_id, parent):
        super().__init__(parent)

        pixel_ratio = QWindow().devicePixelRatio()

        self.pixmap_defaults = QPixmap('resources/{0}_button_defaults.png'.format(button_id))
        self.pixmap_defaults = self.pixmap_defaults.scaledToHeight(
                self.pixmap_defaults.height() * pixel_ratio / 2,
                Qt.SmoothTransformation)
        self.pixmap_defaults.setDevicePixelRatio(pixel_ratio)
		
        self.pixmap_hover = QPixmap('resources/{0}_button_hover.png'.format(button_id))
        self.pixmap_hover = self.pixmap_hover.scaledToHeight(
                self.pixmap_hover.height() * pixel_ratio / 2,
                Qt.SmoothTransformation)
        self.pixmap_hover.setDevicePixelRatio(pixel_ratio)
		
        self.pixmap_press = QPixmap('resources/{0}_button_press.png'.format(button_id))
        self.pixmap_press = self.pixmap_press.scaledToHeight(
                self.pixmap_press.height() * pixel_ratio / 2,
                Qt.SmoothTransformation)
        self.pixmap_press.setDevicePixelRatio(pixel_ratio)
		
        self.menu_focused = self.pixmap_defaults
		
        #self.pressed.connect(self.update)
        #self.released.connect(self.update)
 		

    def focusInEvent(self, event):
        self.menu_focused = self.pixmap_hover		
        self.update()	
		
		
    def focusOutEvent(self, event):
        self.menu_focused = self.pixmap_defaults	
        self.update()			
		
    def paintEvent(self, event):

        if self.menu_focused == None:
           pixmap = self.pixmap_defaults
		   
        if self.isDown(): self.menu_focused = self.pixmap_press		   
		   
        painter = QPainter(self)
        painter.drawPixmap(0, 0, self.menu_focused)

 

 
		
import json
import os
import tkinter as tk
from tkinter import simpledialog
from PIL import Image


def generate_meta_json(width, height, directions):
    png_files = [f for f in os.listdir('.') if f.endswith('.png')]

    states = []
    for file in png_files:
        with Image.open(file) as img:
            img_width, img_height = img.size

        state = {"name": file.replace('.png', '')}
        if (img_width != width or img_height != height) and directions > 1:
            state["directions"] = directions

        states.append(state)

    meta = {
        "version": 1,
        "license": "CC-BY-SA-3.0",
        "copyright": "Python generated",
        "size": {
            "x": width,
            "y": height
        },
        "states": states
    }

    with open('meta.json', 'w') as json_file:
        json.dump(meta, json_file, indent=4)

def ask_user_input():
    root = tk.Tk()
    root.withdraw()

    width = simpledialog.askinteger("Input", "Ширина в пикселях (X):", parent=root, minvalue=1)
    height = simpledialog.askinteger("Input", "Высота в пикселях (Y):", parent=root, minvalue=1)
    directions = simpledialog.askinteger("Input", "Количество направлений:", parent=root, minvalue=0)

    if width is not None and height is not None and directions is not None:
        generate_meta_json(width, height, directions)

ask_user_input()


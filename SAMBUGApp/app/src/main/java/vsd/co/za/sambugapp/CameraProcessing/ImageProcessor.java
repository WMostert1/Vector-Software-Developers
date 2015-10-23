package vsd.co.za.sambugapp.CameraProcessing;

/**
 * Created by Kale-ab on 2015/09/30.
 */
import android.app.Dialog;
import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.os.Environment;
import android.view.Window;

import vsd.co.za.sambugapp.R;


public class ImageProcessor {
    Bitmap original;
    Bitmap digits[][];
    int width;
    int height;
    int cellWidth;
    int cellHeight;

    public ImageProcessor(Context context,Bitmap bitmap){
        this.original = bitmap;
        this.width = original.getWidth();
        this.height = original.getHeight();
        this.cellWidth = this.width/9;
        this.cellHeight = this.height/9;
        this.digits = new Bitmap[9][9];


//        for(int y=0; y<9; y++){
//            for(int x=0; x<9; x++){
//                this.digits[x][y] = Bitmap.createBitmap(cellWidth, cellHeight, Bitmap.Config.ARGB_8888);
//            }
//        }
    }

    public Bitmap process(){
        for(int y=0; y<9; y++){
            for(int x=0; x<9; x++){
                toBlackAndWhite(x,y);
            }
        }
        Bitmap b = Bitmap.createBitmap(width,height,Bitmap.Config.ARGB_8888);
        for(int cellY=0; cellY<9; cellY++){
            for(int cellX=0; cellX<9; cellX++){
                for (int y = cellY*cellHeight; y < height && y < cellY*cellHeight+cellHeight; y++) {
                    for(int x = cellX*cellWidth; x < width && x < cellX*cellWidth+cellWidth ; x++) {
                        b.setPixel(x, y, digits[cellX][cellY].getPixel(x-cellX*cellWidth, y-cellY*cellHeight));
                    }
                }
            }
        }

        return b;
    }

    private void toBlackAndWhite(int cellX, int cellY) {
        int blackestPixel = 1000;
        int whitestPixel = -1;
        long averagePixel = 0;
        for (int y = cellY*cellHeight; y < height && y < cellY*cellHeight+cellHeight; y++) {
            for (int x = cellX*cellWidth; x < width && x < cellX*cellWidth+cellWidth ; x++) {
                int pixel = original.getPixel(x, y);
                int R = Color.red(pixel);
                int G = Color.green(pixel);
                int B = Color.blue(pixel);
                int tempComboPixel = R + G + B;
                if(tempComboPixel<blackestPixel){
                    blackestPixel = tempComboPixel;
                }
                if(tempComboPixel>whitestPixel){
                    whitestPixel = tempComboPixel;
                }
                averagePixel += tempComboPixel;
            }
        }
        averagePixel = averagePixel/(cellWidth*cellHeight);
        int toleranceBlack = (25 * (whitestPixel - blackestPixel))/100;
        int toleranceAverage = (int) (10* (averagePixel/100));
        int white = 0;
        int black = 0;
        for (int y = cellY*cellHeight; y < height && y < cellY*cellHeight+cellHeight; y++){
            for (int x = cellX*cellWidth; x < width && x < cellX*cellWidth+cellWidth ; x++) {
                int pixel = original.getPixel(x, y);
                int R = Color.red(pixel);
                int G = Color.green(pixel);
                int B = Color.blue(pixel);
                int comboPixel = R+G+B;
                if(comboPixel>averagePixel-toleranceAverage && comboPixel<averagePixel+toleranceAverage ){
                    digits[cellX][cellY].setPixel(x-cellX*cellWidth, y-cellY*cellHeight, Color.GREEN);
                }
                else if(comboPixel<=blackestPixel+toleranceBlack){
                    digits[cellX][cellY].setPixel(x-cellX*cellWidth, y-cellY*cellHeight, Color.BLACK);
                    black++;
                }
                else{
                    //digits[cellX][cellY].setPixel(x-cellX*cellWidth, y-cellY*cellHeight, Color.WHITE);
                    digits[cellX][cellY].setPixel(x-cellX*cellWidth, y-cellY*cellHeight, original.getPixel(x, y));
                    white++;
                }
            }
        }
        if(black>white){
            invertColors(digits[cellX][cellY]);
        }


    }

    private void invertColors(Bitmap change){
        int height = change.getHeight();
        int width = change.getWidth();

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if(change.getPixel(x, y)==Color.BLACK){
                    change.setPixel(x, y, Color.WHITE);
                }
                else if(change.getPixel(x, y)==Color.WHITE){
                    change.setPixel(x, y, Color.BLACK);
                }
            }
        }

    }






}
package vsd.co.za.sambugapp.CameraProcessing;

/**
 * Created by Kale-ab on 2015/09/30.
 */
import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Paint.Align;
import android.util.AttributeSet;
import android.view.Display;
import android.view.View;
import android.view.WindowManager;

public class Peephole extends View{

    private int width;
    private int height;
    private int peepSide;
    private int peepSideX;
    private int peepSideY;
    private Paint text;
    private Paint grayArea;
    private Paint square;
    private Bitmap bitmap;

    public int getBoxX1() {
        return boxX1;
    }

    public void setBoxX1(int boxX1) {
        this.boxX1 = boxX1;
    }

    public int getBoxX2() {
        return boxX2;
    }

    public void setBoxX2(int boxX2) {
        this.boxX2 = boxX2;
    }

    public int getBoxY1() {
        return boxY1;
    }

    public void setBoxY1(int boxY1) {
        this.boxY1 = boxY1;
    }

    public int getBoxY2() {
        return boxY2;
    }

    public void setBoxY2(int boxY2) {
        this.boxY2 = boxY2;
    }

    private int boxX1,boxX2,boxY1,boxY2;

    public Peephole(Context context, AttributeSet attrs) {
        super(context, attrs);
        WindowManager wm = (WindowManager) context.getSystemService(Context.WINDOW_SERVICE);
        Display display = wm.getDefaultDisplay();
        width = display.getWidth();
        height = display.getHeight();
        init();
    }

    private void init(){
        text = new Paint();
        text.setColor(Color.GREEN);
        text.setTextSize((height*5)/100);
        text.setTextAlign(Align.CENTER);
        grayArea = new Paint();
        grayArea.setColor(Color.argb(255/3, Color.red(Color.GREEN), Color.green(Color.GREEN), Color.blue(Color.GREEN)));
        square = new Paint();
        square.setColor(Color.GREEN);
        peepSide = (int) (height - text.getTextSize()*2);
        peepSideX = width/2-peepSide/2;
        peepSideY = height/2-peepSide/2;
    }

    public int getPeepSide(){
        return peepSide;
    }

    public int getPeepSideX(){
        return peepSideX;
    }

    public int getPeepSideY(){
        return peepSideY;
    }

    public void setBitmap(Bitmap bitmap){
        this.bitmap = bitmap;
    }

    public void onDraw(Canvas canvas){
        super.onDraw(canvas);
      //  canvas.drawText("Sudoku", width/2, text.getTextSize(), text);
       // canvas.drawRect(0, 0, width/2-peepSide/2, height, grayArea);
      //  canvas.drawRect(width/2+peepSide/2, 0, width, height, grayArea);
       // canvas.drawRect(width/2-peepSide/2,0,width/2+peepSide/2,height/2-peepSide/2, grayArea);
       // canvas.drawRect(width/2-peepSide/2,height/2+peepSide/2,width/2+peepSide/2,height, grayArea);


//        canvas.drawLine(width/4,height/4, width*3/4, height/4, square);
//        canvas.drawLine(width/4,height*3/4, width*3/4,height*3/4, square);
//        canvas.drawLine(width/4, height/4, width/4, height*3/4, square);
//        canvas.drawLine(width*3/4, height/4, width*3/4, height*3/4, square);

//        int minX,maxX,minY,maxY;
//        int padX = width/10;
//        int padY = height/10;
//        minX = 0;
//        maxX =  width;
//        minY = 0;
//        maxY = height;

//        canvas.drawLine(padX,padY, maxX-padX,padY, square);
//        canvas.drawLine(padX,maxY-padY,  maxX-padX,maxY-padY, square);
//        canvas.drawLine(padX, padY, padX, maxY-padY, square);
//        canvas.drawLine(maxX-padX, padY, maxX-padX, maxY-padY, square);

        int minX,maxX,minY,maxY;
        int padX = width*1/8;
        int padY = height*1/8;
        minX = 0;
        maxX =  width*7/8;
        minY = 0;
        maxY = height*7/8;

        canvas.drawLine(padX,padY, maxX,padY, square);
        canvas.drawLine(padX,maxY,  maxX,maxY, square);
        canvas.drawLine(padX, padY, padX, maxY, square);
        canvas.drawLine(maxX, padY, maxX, maxY, square);

        setBoxX1(width/4);
        setBoxX2(width*3/4);
        setBoxY1(height/4);
        setBoxY2(height*3/4);

//        canvas.drawLine(0,0, width, 0, square);
//        canvas.drawLine(0,height, width,height, square);
//        canvas.drawLine(0, 0, 0, height, square);
//        canvas.drawLine(width, 0, width, height, square);

        if(bitmap!=null){
           canvas.drawBitmap(bitmap, peepSideX, peepSideY, new Paint());
        }
    }
}
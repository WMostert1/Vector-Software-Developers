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
    private int minX,maxX,minY,maxY;



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

    /**
     * Draws the square on the camera preview
     * @param canvas
     */
    public void onDraw(Canvas canvas){
        super.onDraw(canvas);
        int Xcentre = (int)(width /2);
        int Ycentre = (int)(height /2);

        //Setting the side length of the square
        int padding = (int)(CustomCamera.SQUARERATIO * Xcentre);
        if(padding > Ycentre){
            padding = (int)(CustomCamera.SQUARERATIO * Ycentre);
        }
        //Beginning of Square
        minX = Xcentre - padding;
        minY = Ycentre - padding;

        maxX =  Xcentre + padding;
        maxY = Ycentre + padding;

        canvas.drawLine(minX,minY, maxX,minY, square);
        canvas.drawLine(minX,maxY,  maxX,maxY, square);
        canvas.drawLine(minX, minY, minX, maxY, square);
        canvas.drawLine(maxX, minY, maxX, maxY, square);

        if(bitmap!=null){
           canvas.drawBitmap(bitmap, peepSideX, peepSideY, new Paint());
        }
    }
}
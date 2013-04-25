Quantae.LessonHubPlotter = {};

Quantae.LessonHubPlotter.ProgressBar = function(paperToDraw, startPoint, height, width, chunks, maxPercent)
{
    this.pencilStrokeWidth = 5;
    this.progressBarHeight = 20;
    this.leftArc = paperToDraw.path(
        this.roundedCornerPath('left', startPoint, {x : startPoint.x, y : (startPoint.y + this.progressBarHeight)})
    );
    
    var arcBox = Quantae.getVisualObjectProperties(this.leftArc);
    
}

Quantae.LessonHubPlotter.ProgressBar.prototype.roundedCornerPath = function(type, pointOne, pointTwo)
{
    var quadrant = (type == 'left') ? "1,0" : "0,1";
    
    var pathStr = "M" + pointOne.x + "," + pointOne.y + " " +
                    "A6, 5.5 " +
                    "0 " +
                    quadrant + " " +
                    pointTwo.x + "," + pointTwo.y;
                    
    return pathStr;
}

Quantae.LessonHubPlotter.PlotPage = function(paperToDraw, pageContents, MarginY)
{
    if (typeof MarginY == 'undefined')
    {
        MarginY = 0;
    }
    
    
}
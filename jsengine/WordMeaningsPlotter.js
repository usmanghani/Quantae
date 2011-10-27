Quantae.plottedWords = [];
Quantae.plottedMeanings = [];

Quantae.MARGIN_WORDS = 50;			// Margin right and left
Quantae.MARGIN_MEANINGS = 20;			// Margin top and bottom
Quantae.MARGIN_GRAMMER_ANALYSIS_TOP = 30;	// Margin top
Quantae.MARGIN_GRAMMER_ANALYSIS_SPOTS = 30;	// Margin top

Quantae.wordsTextFormat = {
				"font-size" : 55,
				"font-family" : "Verdana",
				"fill" : "blue"
			};
			
Quantae.meaningsTextFormat = {
				"font-size" : 15,
				"font-family" : "Verdana",
				"fill" : "red"
			};

Quantae.plotWordMeanings = function(wordList, paperToDraw, startFromLeft, marginFromStart, marginFromTop)
{
	if (typeof marginFromStart == 'undefined')
		marginFromStart = 0;
        
    if (typeof marginFromTop == 'undefined')
		marginFromTop = 100;
	
	var currWord = '';
	var currMeaning = '';
	
	var paper_MaxX = paperToDraw.width;
	var paper_MaxY = paperToDraw.height;
	var paper_MinX = 0;
	var paper_MinY = 0;
	
	var lastX = (startFromLeft) ? (paper_MinX + marginFromStart) : (paper_MaxX - marginFromStart);
	var initY = marginFromTop;
	
	var plottedWord = null;
	var plottedMeaning = null;
	var aboutToPlotWord = null;
	var aboutToPlotMeaning = null;
	
	var meaningX = null;
	var meaningY = null;
	
	for (var i = 0; i < wordList.length; i++)
	{
		currWord = wordList[i].word;
		currMeaning = wordList[i].translation;
		
		if (startFromLeft)
		{
			plottedWord = Quantae.plotText((lastX), initY, currWord, paperToDraw, Quantae.wordsTextFormat);
			lastX += plottedWord.textWidth + Quantae.MARGIN_WORDS;
		}
		else
		{
			aboutToPlotWord = Quantae.getTextProperties(currWord, paperToDraw, Quantae.wordsTextFormat);
			aboutToPlotWord.textWidth;
			
			plottedWord = Quantae.plotText((lastX - (aboutToPlotWord.textWidth)), initY, currWord, paperToDraw, Quantae.wordsTextFormat);
			lastX -= (plottedWord.textWidth + Quantae.MARGIN_WORDS);
		}
		
		aboutToPlotMeaning = Quantae.getTextProperties(currMeaning, paperToDraw, Quantae.meaningsTextFormat);
		
		meaningX = plottedWord.textX + (Number(plottedWord.textWidth / 2) - Number(aboutToPlotMeaning.textWidth / 2));
		meaningY = initY + plottedWord.textHeight + Quantae.MARGIN_MEANINGS;
		
		plottedMeaning = Quantae.plotText(meaningX, meaningY, currMeaning, paperToDraw, Quantae.meaningsTextFormat);
		
		Quantae.plottedMeanings.push(plottedMeaning);
		Quantae.plottedWords.push(plottedWord);
	}
}

Quantae.plotGrammerAnalysis = function(wordList, paperToDraw, startFromLeft, padFromStart)
{
	if (typeof padFromStart == 'undefined')
		padFromStart = 0;
		
	var startY = Quantae.plottedMeanings[0].textHeight + Quantae.plottedMeanings[0].textY + Quantae.MARGIN_GRAMMER_ANALYSIS_TOP;
	
	var currWord = '';
	
	var paper_MaxX = paperToDraw.width;
	var paper_MaxY = paperToDraw.height;
	var paper_MinX = 0;
	var paper_MinY = 0;
	
	var lastX = (startFromLeft) ? (paper_MinX + padFromStart) : (paper_MaxX - padFromStart);
	var initY = 100;
	
	var plottedWord = null;
	var plottedMeaning = null;
	var aboutToPlotWord = null;
	var aboutToPlotMeaning = null;
	
	var meaningX = null;
	var meaningY = null;
	
	for (var i = 0; i < wordList.length; i++)
	{
		currWord = wordList[i].word;
		currMeaning = wordList[i].translation;
		
		if (startFromLeft)
		{
			plottedWord = Quantae.plotText((lastX), initY, currWord, paperToDraw, Quantae.wordsTextFormat);
			lastX += plottedWord.textWidth + Quantae.MARGIN_WORDS;
		}
		else
		{
			aboutToPlotWord = Quantae.getTextProperties(currWord, paperToDraw, Quantae.wordsTextFormat);
			// aboutToPlotWord.textWidth;
			
			plottedWord = Quantae.plotText((lastX - (aboutToPlotWord.textWidth)), initY, currWord, paperToDraw, Quantae.wordsTextFormat);
			lastX -= (plottedWord.textWidth + Quantae.MARGIN_WORDS);
		}
		
		aboutToPlotMeaning = Quantae.getTextProperties(currMeaning, paperToDraw, Quantae.meaningsTextFormat);
		
		meaningX = plottedWord.textX + (Number(plottedWord.textWidth / 2) - Number(aboutToPlotMeaning.textWidth / 2));
		meaningY = initY + plottedWord.textHeight + Quantae.MARGIN_MEANINGS;
		
		plottedMeaning = Quantae.plotText(meaningX, meaningY, currMeaning, paperToDraw, Quantae.meaningsTextFormat);
		
		Quantae.plottedMeanings.push(plottedMeaning);
		Quantae.plottedWords.push(plottedWord);
	}
}
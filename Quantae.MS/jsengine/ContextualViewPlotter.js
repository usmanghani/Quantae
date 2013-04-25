Quantae.ContextualViewPlotter = {};

Quantae.ContextualViewPlotter.PlottedWords = [];
Quantae.ContextualViewPlotter.PlottedWordMargins = [];
Quantae.ContextualViewPlotter.TopMarginSize = 3;
Quantae.ContextualViewPlotter.States = [];
Quantae.ContextualViewPlotter.MaxStateNum = 0;
Quantae.ContextualViewPlotter.StateTopMargins = [];

Quantae.ContextualViewPlotter.PlotPage = function(paperToDraw, data, leftToRight)
{
    var word = null;
    var wordTop = null;
    var wordBottom = null;
    var wordDebug = null;
    
    if (leftToRight)
    {
        for (var i = 0; i < data.grammarEntries.length; i++)
        {
            word = document.createElement('div');
            word.setAttribute('class', 'word');
            
            wordTop = document.createElement('div');
            wordTop.setAttribute('class', 'word-top');
            wordTop.innerHTML = data.grammarEntries[i].word;
            
            wordBottom = document.createElement('div');
            wordBottom.setAttribute('class', 'word-bottom');
            wordBottom.innerHTML = data.grammarEntries[i].translation;
            
            word.appendChild(wordTop);
            word.appendChild(wordBottom);
            
            if (Quantae.DebugMode == 1)
            {
                wordDebug = document.createElement('div');
                wordDebug.setAttribute('style', 'font-size: xx-large;');
                wordDebug.innerHTML = i;
                word.appendChild(wordDebug);
            }
            
            // paperToDraw.appendChild(word);
            Quantae.ContextualViewPlotter.PlottedWords.push(word);
            
            Quantae.ContextualViewPlotter.States[i] = [];
            Quantae.ContextualViewPlotter.MaxStateNum++;
        }
    }
    else
    {
        for (var i = (data.grammarEntries.length - 1); i >= 0; i--)
        {
            word = document.createElement('div');
            word.setAttribute('class', 'word');
            
            wordTop = document.createElement('div');
            wordTop.setAttribute('class', 'word-top');
            wordTop.innerHTML = data.grammarEntries[i].word;
            
            wordBottom = document.createElement('div');
            wordBottom.setAttribute('class', 'word-bottom');
            wordBottom.innerHTML = data.grammarEntries[i].translation;
            
            word.appendChild(wordTop);
            word.appendChild(wordBottom);
            
            if (Quantae.DebugMode == 1)
            {
                wordDebug = document.createElement('div');
                wordDebug.setAttribute('style', 'font-size: xx-large;');
                wordDebug.innerHTML = i;
                word.appendChild(wordDebug);
            }
            
            // paperToDraw.appendChild(word);
            Quantae.ContextualViewPlotter.PlottedWords.push(word);
            
            Quantae.ContextualViewPlotter.States[i] = [];
            Quantae.ContextualViewPlotter.MaxStateNum++;
        }
    }
    
    Quantae.ContextualViewPlotter.CreateGroups(paperToDraw, data.contextualAnalysis, leftToRight);
}

Quantae.ContextualViewPlotter.AlignStates = function(start, end)
{
    var temp;
    if (start > end)
    {
        temp = start;
        start = end;
        end = temp;
    }
    
    return {start : start, end : end};
}

Quantae.ContextualViewPlotter.FindStates = function(start, end)
{
    var startIndex = 0;
    var endIndex = 0;
    
    startIndex = Quantae.ContextualViewPlotter.States['all'].indexOf(start);
    endIndex = Quantae.ContextualViewPlotter.States['all'].indexOf(end);
    
    if (startIndex != -1 && endIndex != -1)
    {
        var indexInfo = Quantae.ContextualViewPlotter.AlignStates(startIndex, endIndex);
        return {
            state : 'all',
            startIndex : indexInfo.start,
            endIndex : indexInfo.end
        };
    }
    
    for (var i = 0; i < Quantae.ContextualViewPlotter.States.length; i++)
    {
        startIndex = Quantae.ContextualViewPlotter.States[i].indexOf(start);
        endIndex = Quantae.ContextualViewPlotter.States[i].indexOf(end);
        
        if (startIndex != -1 && endIndex != -1)
        {
            var indexInfo = Quantae.ContextualViewPlotter.AlignStates(startIndex, endIndex);
            return {
                state : i,
                startIndex : indexInfo.start,
                endIndex : indexInfo.end
            };
        }
    }
}

Quantae.ContextualViewPlotter.EvaluateTopMargins = function(currentState, currentMargin, level, isLeftToRight)
{
    var curr = null;
    var levelClassName = null;
    var mainDiv = null;
    var nextLevel = 0;
    
    if (level == 0)
    {
        mainDiv = document.createElement('div');
        mainDiv.setAttribute('class', 'base');
        nextLevel = 1;
    }
    else if (level == 1)
    {
        mainDiv = document.createElement('div');
        mainDiv.setAttribute('class', 'level-1');
        nextLevel = 2;
    }
    else
    {
        mainDiv = document.createElement('div');
        mainDiv.setAttribute('class', 'level-2');
        nextLevel = 2;
    }
    
    var i = (isLeftToRight) ? 0 : (currentState.length - 1);
    var getNext = true;
    
    while (getNext)
    {
        curr = currentState[i];
        Quantae.ContextualViewPlotter.StateTopMargins[curr] = currentMargin;
        if (Quantae.ContextualViewPlotter.States[curr].length > 0)
        {
            //console.log(Quantae.ContextualViewPlotter.States[curr] + ' ' + (currentMargin + Quantae.ContextualViewPlotter.TopMarginSize));
            mainDiv.appendChild(Quantae.ContextualViewPlotter.EvaluateTopMargins(Quantae.ContextualViewPlotter.States[curr], (currentMargin - Quantae.ContextualViewPlotter.TopMarginSize), nextLevel));
        }
        else
        {
            Quantae.ContextualViewPlotter.PlottedWords[curr].childNodes[0].style.marginTop = currentMargin + "px";
            mainDiv.appendChild(Quantae.ContextualViewPlotter.PlottedWords[curr]);
        }
        
        if (isLeftToRight)
        {
            i++;
            getNext = (i < currentState.length);
        }
        else
        {
            getNext = (i > 0);
            i--;
        }
    }
    
    return mainDiv;
}

Quantae.ContextualViewPlotter.CreateGroups = function(paperToDraw, groups, leftToRight)
{
    Quantae.ContextualViewPlotter.States['all'] = [];
    
    for (var i = 0; i < Quantae.ContextualViewPlotter.States.length; i++)
    {
        Quantae.ContextualViewPlotter.States['all'].push(i);
    }
    
    Quantae.ContextualViewPlotter.PlottedGroups = Quantae.ContextualViewPlotter.PlottedWords.slice();
    var stateInfo = null;
    var newState = null;
    
    var statesRightArr = null;
    
    for (var i = 0; i < groups.length; i++)
    {
        stateInfo = Quantae.ContextualViewPlotter.FindStates(groups[i].startIndex, groups[i].endIndex);
        newState = Quantae.ContextualViewPlotter.States[stateInfo.state].splice(stateInfo.startIndex, (stateInfo.endIndex - stateInfo.startIndex + 1));
        Quantae.ContextualViewPlotter.States[Quantae.ContextualViewPlotter.MaxStateNum] = newState;
        
        /* adding the new state in old state array */
        statesRightArr = Quantae.ContextualViewPlotter.States[stateInfo.state].splice(stateInfo.startIndex);
        Quantae.ContextualViewPlotter.States[stateInfo.state].push(Quantae.ContextualViewPlotter.MaxStateNum++);
        if (statesRightArr.length > 0)
        {
            for (var j = 0; j < statesRightArr.length; j++)
            {
                Quantae.ContextualViewPlotter.States[stateInfo.state].push(statesRightArr[j]);
            }
        }
        /* adding the new state in old state array */
    }
    
    paperToDraw.appendChild(
        Quantae.ContextualViewPlotter.EvaluateTopMargins(Quantae.ContextualViewPlotter.States['all'], (groups.length * Quantae.ContextualViewPlotter.TopMarginSize), 0, leftToRight)
    );
    
    //Quantae.ContextualViewPlotter.finalDiv = ;
}
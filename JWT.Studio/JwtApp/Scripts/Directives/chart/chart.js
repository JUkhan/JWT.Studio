class chart{
  
 	constructor(){   
      this.restrict='ECA';       
      this.link=function(scope, iEle, attr){       
        	
          var plot1 = jQuery.jqplot (attr.id, [scope.data], 
            { 
              seriesDefaults: {                
                renderer: jQuery.jqplot.PieRenderer, 
                rendererOptions: {                  
                  showDataLabels: true
                }
              }, 
              legend: { show:true, location: 'e' }
            }
          );
      };
      
      this.scope={
        data:'='
      };
     
	}  
    static builder(){ 
      return  new chart(); 
    }
}

export default chart;
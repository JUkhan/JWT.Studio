
export default class BaseCtrl{

    constructor()
    {

    }
    async(g){
        let it=g(),ret;
        (function iterate(val){
            ret=it.next(val);
            if(!ret.done){             
                if(ret.value){
                    if('success' in ret.value){
                        ret.value.success(iterate);
                    }
                    else{
                        iterate(ret.value);
                    }
                }
            }
        })();
    }
} 
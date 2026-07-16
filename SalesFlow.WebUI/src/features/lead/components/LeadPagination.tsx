type Props = {

  page:number;

  totalPages:number;

  hasPrevious:boolean;

  hasNext:boolean;

  onPageChange:(page:number)=>void;

};

function LeadPagination({

  page,

  totalPages,

  hasPrevious,

  hasNext,

  onPageChange,

}:Props){

return(

<div className="flex items-center justify-between">

<p className="text-sm text-slate-500">

Page {page} / {totalPages}

</p>

<div className="flex gap-3">

<button

disabled={!hasPrevious}

onClick={()=>onPageChange(page-1)}

className="rounded-xl border px-4 py-2 disabled:opacity-50"

>

Previous

</button>

<button

disabled={!hasNext}

onClick={()=>onPageChange(page+1)}

className="rounded-xl border px-4 py-2 disabled:opacity-50"

>

Next

</button>

</div>

</div>

);

}

export default LeadPagination;
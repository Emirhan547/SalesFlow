import { useState } from "react";

import ConfirmDialog from "@/components/ui/ConfirmDialog";

type Props = {

  open:boolean;

  loading:boolean;

  onCancel:()=>void;

  onConfirm:(options:{

    customerType:number;

    createInitialDeal:boolean;

    createInitialMeeting:boolean;

    createInitialTask:boolean;

  })=>Promise<void>;

};

function LeadConvertDialog({

  open,

  loading,

  onCancel,

  onConfirm,

}:Props){

const[customerType,setCustomerType]=
useState(1);

const[deal,setDeal]=
useState(true);

const[meeting,setMeeting]=
useState(false);

const[task,setTask]=
useState(true);

return(

<ConfirmDialog

open={open}

loading={loading}

title="Convert Lead"

description={

<div className="space-y-4">

<select

value={customerType}

onChange={e=>

setCustomerType(Number(e.target.value))

}

className="w-full rounded-xl border p-3"

>

<option value={1}>

Individual

</option>

<option value={2}>

Corporate

</option>

</select>

<label className="flex gap-2">

<input

type="checkbox"

checked={deal}

onChange={e=>

setDeal(e.target.checked)}

 />

Create Deal

</label>

<label className="flex gap-2">

<input

type="checkbox"

checked={meeting}

onChange={e=>

setMeeting(e.target.checked)}

 />

Create Meeting

</label>

<label className="flex gap-2">

<input

type="checkbox"

checked={task}

onChange={e=>

setTask(e.target.checked)}

 />

Create Task

</label>

</div>

}

onCancel={onCancel}

onConfirm={()=>

onConfirm({

customerType,

createInitialDeal:deal,

createInitialMeeting:meeting,

createInitialTask:task,

})

}

/>

);

}

export default LeadConvertDialog;
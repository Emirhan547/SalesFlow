import {

Download,

FileText,

Plus,

} from "lucide-react";

import { Button } from "@/components/ui/button";

type Props={

onCreate:()=>void;

onExcel:()=>void;

onPdf:()=>void;

};

function LeadHeader({

onCreate,

onExcel,

onPdf,

}:Props){

return(

<div className="flex flex-wrap items-center justify-between gap-4">

<div>

<h1 className="text-4xl font-bold">

Leads

</h1>

<p className="mt-2 text-slate-500">

Manage your sales leads.

</p>

</div>

<div className="flex gap-3">

<Button

variant="outline"

onClick={onPdf}

>

<FileText

size={18}

className="mr-2"

/>

Export PDF

</Button>

<Button

variant="outline"

onClick={onExcel}

>

<Download

size={18}

className="mr-2"

/>

Export Excel

</Button>

<Button

onClick={onCreate}

>

<Plus

size={18}

className="mr-2"

/>

New Lead

</Button>

</div>

</div>

);

}

export default LeadHeader;
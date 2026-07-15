import { useState } from "react";

import { createCustomer }
    from "../services/customerService";

import type {
CreateCustomerRequest
}
from "../types/CreateCustomerRequest";

export function useCreateCustomer(){

const [loading,setLoading]=
useState(false);

async function submit(
request:CreateCustomerRequest
){

setLoading(true);

try{

await createCustomer(request);

return true;

}
finally{

setLoading(false);

}

}

return{

loading,

submit

};

}
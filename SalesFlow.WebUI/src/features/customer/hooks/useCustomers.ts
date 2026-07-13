import {useEffect,useState} from "react";

import type { Customer } from "@/features/customer/types/Customer";
import { getCustomers } from "../services/customerService";
import type { PagedResult } from "@/types/PagedResult";



export function useCustomers(){

    const [customers,setCustomers]=
    useState<PagedResult<Customer>>();

    const [loading,setLoading]=
    useState(true);

    const [page,setPage]=
    useState(1);

    const [pageSize]=
    useState(10);

    const [search,setSearch]=
    useState("");

    useEffect(()=>{

        load();

    },[page,search]);

    async function load(){

        setLoading(true);

        try{

            const response=

            await getCustomers(

                page,

                pageSize,

                search

            );

            setCustomers(response);

        }

        finally{

            setLoading(false);

        }

    }

    return{

        customers,

        loading,

        page,

        setPage,

        search,

        setSearch,

        refresh:load

    };

}
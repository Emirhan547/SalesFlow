export interface DashboardDto {
  summary: DashboardSummaryDto;
  sales: DashboardSalesDto;
  leads: DashboardLeadDto;
  tasks: DashboardTaskDto;
  meetings: DashboardMeetingDto;
  recent: DashboardRecentDto;
}

export interface DashboardSummaryDto {
  totalCustomers: number;
  totalLeads: number;
  totalDeals: number;
  totalMeetings: number;
  totalTasks: number;
}

export interface DashboardSalesDto {
  pipelineAmount: number;
  wonAmount: number;
  wonDeals: number;
  lostDeals: number;
  activeDeals: number;
  monthlySales:MonthlySalesDto[];
}

export interface DashboardLeadDto {
  new: number;
  contacted: number;
  qualified: number;
  converted: number;
  lost: number;
}

export interface DashboardTaskDto {
  pending: number;
  inProgress: number;
  completed: number;
  cancelled: number;
}

export interface DashboardMeetingDto {
  today: number;
  thisWeek: number;
  scheduled: number;
  completed: number;
}

export interface DashboardRecentDto {
  customers: RecentCustomerDto[];
  leads: RecentLeadDto[];
  deals: RecentDealDto[];
  upcomingMeetings: UpcomingMeetingDto[];
  upcomingTasks: UpcomingTaskDto[];
}

export interface RecentCustomerDto {
  id: number;
  fullName: string;
  companyName: string;
  createdDate: string;
}

export interface RecentLeadDto {
  id: number;
  firstName: string;
  lastName: string;
  companyName: string;
}

export interface RecentDealDto {
  id: number;
  title: string;
  amount: number;
}

export interface UpcomingMeetingDto {
  id: number;
  title: string;
  startDate: string;
}

export interface UpcomingTaskDto {
  id: number;
  title: string;
  dueDate: string;
}
export interface ResultActivityLogDto {
    id:number;
    action:string;
    entityName:string;
    entityId:number;
    description:string;
    userId:number | null;
    userName:string | null;
    createdDate:string;
}
export interface MonthlySalesDto{
    month:string;
    amount:number;
}
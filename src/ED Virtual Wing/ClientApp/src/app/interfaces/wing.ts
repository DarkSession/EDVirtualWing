export interface Wing {
    WingId: string;
    Name: string;
}

export enum WingStatus {
    Deleted = 0,
    Active,
}

export enum WingJoinRequirement {
    Invite = 0,
    Approval,
}
export type Role = "Admin" | "User";

export interface UserResponse {
  id: string;
  name: string;
  email: string;
  createdAt: string;
  updatedAt: string;
  role: Role;
}

export interface CreateUserRequest {
  name: string;
  email: string;
  password: string;
  role: Role;
}

export interface UpdateUserRequest {
  name: string;
  email: string;
  password: string;
  role: Role;
}

export interface BirdResponse {
  id: string;
  name: string;
  description: string;
  velocity: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateBirdRequest {
  name: string;
  description: string;
  velocity: number;
}

export interface UpdateBirdRequest {
  name: string;
  description: string;
  velocity: number;
}

export type AttachmentType = "Photo" | "File";

export interface MessageResponse {
  id: string;
  senderId: string;
  receiverId: string;
  text: string;
  attachmentUrl: string | null;
  attachmentType: AttachmentType | null;
  senderLatitude: number;
  senderLongitude: number;
  receiverLatitude: number;
  receiverLongitude: number;
  createdAt: string;
}

export interface CreateMessageRequest {
  senderId: string;
  receiverId: string;
  text: string;
  attachmentUrl: string | null;
  attachmentType: AttachmentType | null;
  senderLatitude: number;
  senderLongitude: number;
  receiverLatitude: number;
  receiverLongitude: number;
}

export interface PaginatedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
}

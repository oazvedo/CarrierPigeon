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
  birdId: string;
  text: string;
  attachmentUrl: string | null;
  attachmentType: AttachmentType | null;
  senderLatitude: number;
  senderLongitude: number;
  receiverLatitude: number;
  receiverLongitude: number;
  distance: number;
  estimatedDeliveryMinutes: number;
  createdAt: string;
}

export interface CreateMessageRequest {
  senderId: string;
  receiverId: string;
  birdId: string;
  text: string;
  attachmentUrl: string | null;
  attachmentType: AttachmentType | null;
}

export interface AddressResponse {
  id: string;
  userId: string;
  cep: string;
  number: string | null;
  street: string | null;
  neighborhood: string | null;
  local: string | null;
  uf: string | null;
  state: string | null;
  region: string | null;
  ddd: string | null;
}

export interface CreateAddressRequest {
  userId: string;
  cep: string;
  number: string | null;
}

export interface UpdateAddressRequest {
  userId: string;
  cep: string;
  number: string | null;
}

export interface PaginatedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
}

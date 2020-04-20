import {http} from '../tools/axios';

export const upsertGroup = (details) =>
	http.post('api/groups/upsert', details).then(x => x.data);

export const deleteGroup = id =>
	http.delete(`api/groups/${id}`);

export const groupMembers = id =>
	http.get(`api/groups/${id}/members`).then(x => x.data);

export const listGroups = () =>
	http.get('api/groups').then(x => x.data);

export const listGroupInvitations = () =>
	http.get('api/groups/invitations').then(x => x.data);

export const acceptInvitation = groupId =>
	http.post(`api/groups/invitations/accept/${groupId}`);

export const invitePersonInGroup = (group, user) =>
	http.post(`api/groups/${group}/invite/${user}`);

export const deletePersonFromGroup = (group, user) =>
	http.post(`api/groups/${group}/delete/${user}`);

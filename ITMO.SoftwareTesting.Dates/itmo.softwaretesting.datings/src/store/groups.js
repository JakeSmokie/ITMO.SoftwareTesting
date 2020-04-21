import Vue from 'vue';
import {
	acceptInvitation,
	deleteGroup,
	deletePersonFromGroup,
	groupDetails,
	invitePersonInGroup,
	listGroupInvitations,
	listGroups,
	upsertGroup,
} from '../services/groups.service';

const state = {
	groups: [],
	invitations: [],
};

const getters = {
	myGroups: state => state.groups.filter(x => x.owner),
};

const mutations = {
	upsertGroup(state, group) {
		if (state.groups.some(x => x.id === group.id)) {
			state.groups = state.groups.filter(x => x.id !== group.id);
		}

		state.groups.push(group);
		state.groups.sort((x, y) => x.id - y.id);
	},

	setGroups(state, groups) {
		state.groups = groups;
	},

	deleteGroup(state, id) {
		state.groups = state.groups.filter(x => x.id !== id);
	},

	loadGroupDetails(state, [group, details]) {
		const index = state.groups.findIndex(x => x.id === group.id);

		Vue.set(state.groups, index, {
			...group,
			...details,
		});
	},

	invitePersonInGroup(state, {userId, userNickname, group}) {
		const newGroup = state.groups.find(x => x.id === group.id);
		newGroup.invites.push({id: userId, nickname: userNickname, invited: true});

		state.groups = [...state.groups];
	},

	loadGroupInvitations(state, invitations) {
		state.invitations = invitations;
	},

	acceptInvitation(state, group) {
		state.invitations = state.invitations.filter(x => x.id !== group.id);
		state.groups.push({
			...group,
			invites: [],
		});
	},

	deletePersonFromGroup(state, {group, user}) {
		const index = state.groups.findIndex(x => x.id === group);
		const newGroup = state.groups.find(x => x.id === group);

		Vue.set(state.groups, index, {
			...newGroup,
			members: newGroup.members.filter(x => x.id !== user),
		});
	},
};

const actions = {
	async upsertGroup({commit}, group) {
		const id = await upsertGroup(group);
		commit(mutations.upsertGroup.name, {
			...group,
			id,
			owner: true,
		});
	},

	async deleteGroup({commit}, group) {
		commit(mutations.deleteGroup.name, group.id);
		await deleteGroup(group.id);
	},

	async listGroups({commit}) {
		commit(mutations.setGroups.name, await listGroups());
	},

	async loadGroupDetails({commit}, group) {
		commit(mutations.loadGroupDetails.name, [group, await groupDetails(group.id)]);
	},

	async invitePersonInGroup({commit}, {userId, userNickname, group}) {
		await invitePersonInGroup(group.id, userId);
		commit(mutations.invitePersonInGroup.name, {userId, userNickname, group});
	},

	async loadGroupInvitations({commit}) {
		commit(mutations.loadGroupInvitations.name, await listGroupInvitations());
	},

	async acceptInvitation({commit, dispatch}, group) {
		await acceptInvitation(group.id);
		commit(mutations.acceptInvitation.name, group);

		await dispatch(actions.loadGroupDetails.name, group);
	},

	async deletePersonFromGroup({commit}, {group, user}) {
		await deletePersonFromGroup(group, user);
		commit(mutations.deletePersonFromGroup.name, {group, user});
	},
};

export const groupsModule = {
	namespaced: true,
	state,
	mutations,
	actions,
	getters,
};

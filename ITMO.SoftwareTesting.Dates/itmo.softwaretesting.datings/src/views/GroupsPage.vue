<template>
	<b-container>
		<group-creation-form/>
		<group-invitations/>
		<groups-list/>
	</b-container>
</template>

<script>
	import GroupCreationForm from '../components/groups/GroupCreationForm';
	import GroupsList from '../components/groups/GroupsList';
	import GroupInvitations from '../components/groups/GroupInvitations';

	export default {
		name: 'GroupsPage',
		components: {GroupInvitations, GroupsList, GroupCreationForm},
	};
</script>


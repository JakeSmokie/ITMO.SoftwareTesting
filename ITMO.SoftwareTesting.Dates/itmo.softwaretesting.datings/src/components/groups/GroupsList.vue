<template>
	<div class="pt-4 text-left" v-if="groups.length > 0">
		<h4 class="px-4">Мои группы:</h4>
		<b-list-group class="pt-2">
			<group-list-item :group="item" v-for="item in groups" :key="item.id"/>
		</b-list-group>
	</div>
</template>

<script>
	import {mapActions, mapState} from 'vuex';
	import GroupListItem from './GroupListItem';

	export default {
		name: 'GroupsList',
		components: {GroupListItem},

		async created() {
			await this.listGroups();
		},

		computed: {
			...mapState('groups', ['groups']),
		},

		methods: {
			...mapActions('groups', ['listGroups']),
		},
	};
</script>

